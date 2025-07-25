using Gender.GrpcService.DuyVK.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using System.Text;

namespace Gender.GrpcClient.DuyVK
{
    public class App
    {
        // ==========================
        // === Fields
        // ==========================

        private static string SERVER_ADDRESS = "https://localhost:7121";
        private GrpcChannel _channel = null;
        private Metadata _metadata = null;
        private AuthDuyVKGRPC.AuthDuyVKGRPCClient _authClient = null;
        private MenstrualCycleReminderDuyVKGRPC.MenstrualCycleReminderDuyVKGRPCClient _reminderClient = null;
        private ReminderCategoryDuyVKGRPC.ReminderCategoryDuyVKGRPCClient _categoryClient = null;

        // ======================================
        // === Main Program Logic
        // ======================================

        public async Task RunAsync()
        {
            // =========== Initialize gRPC Clients & Channels ===========
            Initialize();

            // =========== Showing Menu =================================

            // Login
            bool isLoggined = false;
            while (!isLoggined)
            {
                await LoginLoopAsync();
                isLoggined = true;

                // Menu
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nSelect an option:");
                    Console.WriteLine(" 0) List all categories");
                    Console.WriteLine(" 1) List all reminders");
                    Console.WriteLine(" 2) List paged reminders");
                    Console.WriteLine(" 3) Search reminders");
                    Console.WriteLine(" 4) Get reminder by ID");
                    Console.WriteLine(" 5) Create a reminder");
                    Console.WriteLine(" 6) Update a reminder");
                    Console.WriteLine(" 7) Delete a reminder");
                    Console.WriteLine(" 8) Exit");
                    Console.Write("Your choice → ");

                    switch (Console.ReadLine()!.Trim())
                    {
                        case "0": await ListAllCategories(); break;
                        case "1": await ListAllReminders(); break;
                        case "2": await ListAllPagedReminders(); break;
                        case "3": await SearchReminders(); break;
                        case "4": await GetReminderById(); break;
                        case "5": await CreatReminderAsync(); break;
                        case "6": await UpdateReminderAsync(); break;
                        case "7": await DeleteReminderAsync(); break;
                        case "8": exit = true; break;

                        default: Console.WriteLine("Invalid option."); break;
                    }
                }
            }

            Console.WriteLine("Bye Bye!! ");
        }



        // ==========================
        // === Methods
        // ==========================

        /**
         * Initialize gRPC clients
         */
        public void Initialize()
        {
            // Establish gRPC channel and bypass SSL validation
            _channel = GrpcChannel.ForAddress(SERVER_ADDRESS, new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // Bypass SSL validation
                }
            });

            // Initialize gRPC channel
            _metadata = new Metadata();
            _authClient = new AuthDuyVKGRPC.AuthDuyVKGRPCClient(_channel);
            _reminderClient = new MenstrualCycleReminderDuyVKGRPC.MenstrualCycleReminderDuyVKGRPCClient(_channel);
            _categoryClient = new ReminderCategoryDuyVKGRPC.ReminderCategoryDuyVKGRPCClient(_channel);
        }

        /**
         * Loop for prompting user for login credentials
         */
        private async Task LoginLoopAsync()
        {
            while (true)
            {
                Console.Write("Username: ");
                var user = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Password: ");
                var pass = ReadPassword();

                try
                {
                    // call login method client
                    var rsp = await _authClient.LoginAsync(new LoginDuyVKRequest
                    {
                        Username = user,
                        Password = pass,
                    });

                    // extract token and expiration time
                    _metadata.Add(new Metadata.Entry("authorization", $"Bearer {rsp.Token}"));
                    Console.WriteLine($"\n\n\t Logged in. Token valid for {rsp.ExpiresIn}s\n\n");
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\n\t Invalid credentials, please try again.\n\n");
                }
            }
        }

        /**
         * Read password from consolek
         */
        private string ReadPassword()
        {
            var pwd = new StringBuilder();

            // read keys until Enter is pressed
            ConsoleKeyInfo key;
            while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (key.Key == ConsoleKey.Backspace && pwd.Length > 0)
                {
                    pwd.Remove(pwd.Length - 1, 1);
                    Console.Write("\b \b"); // Remove last character from console
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    pwd.Append(key.KeyChar);
                    Console.Write("*"); // Show asterisk for each character
                }
            }

            return pwd.ToString();
        }

        // ==========================
        // === Categories Reminders
        // ==========================

        /**
         * List the categories
         */
        private async Task ListAllCategories()
        {
            try
            {
                Console.WriteLine("\n\t-- Reminder Categories --\n");
                var rsp = await _categoryClient.GetAllAsync(new ReminderCategoryDuyVKEmptyRequest(), _metadata);

                // Print the list of categories
                Helpers.PrintCategoryList(rsp.Items);
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }

        // ==========================
        // === Reminders
        // ==========================

        private async Task GetReminderById()
        {
            try
            {
                Console.Write("\nEnter reminder ID: ");
                if (!int.TryParse(Console.ReadLine(), out var id))
                {
                    Console.WriteLine("Invalid ID.");
                    return;
                }

                var r = await _reminderClient.GetByIdAsync(
                    new MenstrualCycleReminderDuyVKIdRequest { MenstrualCycleReminderDuyVKid = id },
                    _metadata);

                // List the reminder
                Helpers.PrintReminderList(new MenstrualCycleReminderDuyVK[] { r });
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.NotFound)
            {
                Console.WriteLine("Reminder not found.");
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }

        /**
         * List all reminders
         */
        private async Task ListAllReminders()
        {
            try
            {
                Console.WriteLine("\n\t-- All Reminders --\n");
                var rsp = await _reminderClient.GetAllAsync(new MenstrualCycleReminderDuyVKEmptyRequest(), _metadata);

                // List table of reminders
                Helpers.PrintReminderList(rsp.Items);
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }

        /**
         * Search reminders by title, importance score, and color code
         */
        private async Task SearchReminders()
        {
            try
            {
                int page = Helpers.PromptInt("Page number (default is 1)") ?? 1;
                int size = Helpers.PromptInt("Page size (default is 5)") ?? 5;
                string title = Helpers.Prompt("Title (optional, leave empty for all)");
                double? score = Helpers.PromptDouble("Important Score (optional, leave empty for all)");
                string colorCode = Helpers.Prompt("Color Code (optional, leave empty for all)");

                var req = new GrpcService.DuyVK.Protos.MenstrualCycleReminderDuyVKSearchRequest()
                {
                    CurrentPage = page,
                    PageSize = size,
                    ColorCode = colorCode,
                    ImportanceScore = score,
                    Title = title,
                };

                var rsp = await _reminderClient.SearchAsync(req, _metadata);
                Console.WriteLine($"\n\n\tPage {rsp.CurrentPage}/{rsp.TotalPages} ({rsp.TotalItems} total):");

                // List table of reminders
                Helpers.PrintReminderList(rsp.Items);
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }

        /**
         * List reminders with pagination
         * Input: page number and page size
         */
        private async Task ListAllPagedReminders()
        {
            try
            {
                int page = Helpers.PromptInt("Page number (default is 1)") ?? 1;
                int size = Helpers.PromptInt("Page size (default is 5)") ?? 5;

                var req = new GrpcService.DuyVK.Protos.SearchRequest()
                {
                    CurrentPage = page,
                    PageSize = size,
                };

                var rsp = await _reminderClient.GetAllPagedAsync(req, _metadata);
                Console.WriteLine($"\n\n\tPage {rsp.CurrentPage}/{rsp.TotalPages} ({rsp.TotalItems} total):");

                // List table of reminders
                Helpers.PrintReminderList(rsp.Items);
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }


        /**
         * Create New Reminder
         */
        private async Task CreatReminderAsync()
        {
            try
            {
                await ListAllCategories();
                Console.WriteLine("\n\t-- Create New Reminder --\n");

                var msg = new MenstrualCycleReminderDuyVK
                {
                    ReminderCategoryDuyVKid = Helpers.PromptInt("Category ID") ?? 0,
                    Title = Helpers.Prompt("Title"),
                    Note = Helpers.Prompt("Note"),
                    ReminderDate = Helpers.Prompt("Reminder date (yyyy-MM-dd)"),
                    SentAt = Helpers.Prompt("Sent at (yyyy-MM-dd or empty)"),
                    IsSent = bool.TryParse(Helpers.Prompt("Is sent (true/false)"), out var f) && f,
                    RepeatInterval = Helpers.PromptInt("Repeat interval") ?? 0,
                    ImportanceScore = double.TryParse(Helpers.Prompt("Importance score"), out var d) ? d : 0,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    UpdatedAt = DateTime.UtcNow.ToString("o")
                };

                var rsp = await _reminderClient.CreateAsync(msg, _metadata);
                Console.WriteLine($"\n\n\tCreate successful? {rsp.IsSuccessful}");
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }

        /**
         * Update Reminders
         */
        private async Task UpdateReminderAsync()
        {
            try
            {
                await ListAllReminders();

                Console.WriteLine("\n\t-- Update Reminder --\n");

                // prompt id
                var id = Helpers.PromptInt("ID to update");

                if (id == null || id <= 0)
                    throw new InvalidDataException("Pleae enter id greater than 0. Please try again");

                var msg = new MenstrualCycleReminderDuyVK
                {
                    MenstrualCycleReminderDuyVKid = id.Value,
                    ReminderCategoryDuyVKid = Helpers.PromptInt("Category ID") ?? 0,
                    Title = Helpers.Prompt("Title"),
                    Note = Helpers.Prompt("Note"),
                    ReminderDate = Helpers.Prompt("Reminder date (yyyy-MM-dd)"),
                    SentAt = Helpers.Prompt("Sent at (yyyy-MM-dd or empty)"),
                    IsSent = bool.TryParse(Helpers.Prompt("Is sent (true/false)"), out var f) && f,
                    RepeatInterval = Helpers.PromptInt("Repeat interval") ?? 0,
                    ImportanceScore = double.TryParse(Helpers.Prompt("Importance score"), out var d) ? d : 0,
                    CreatedAt = DateTime.UtcNow.ToString("o"),
                    UpdatedAt = DateTime.UtcNow.ToString("o")
                };

                var rsp = await _reminderClient.UpdateAsync(msg, _metadata);
                Console.WriteLine($"\n\n\tUpdate successful? {rsp.IsSuccessful}");
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }

        /**
         * Delete By Id
         */
        private async Task DeleteReminderAsync()
        {
            try
            {
                await ListAllReminders();

                Console.WriteLine("\n\t-- Delete Reminder --\n");
                Console.Write("\nEnter ID to delete: ");

                if (!int.TryParse(Console.ReadLine(), out var id))
                {
                    Console.WriteLine("\n\n\tInvalid ID.");
                    return;
                }

                var rsp = await _reminderClient.DeleteAsync(
                    new MenstrualCycleReminderDuyVKIdRequest { MenstrualCycleReminderDuyVKid = id },
                    _metadata);

                Console.WriteLine($"\n\n\tDelete successful? {rsp.IsSuccessful}");
            }
            catch (RpcException e) when (e.StatusCode == StatusCode.Unauthenticated)
            {
                Console.WriteLine($"\n\n\tYou are not allowed to access this resource. Please try again.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n\n\tUnexpected error: {e.Message}");
            }
        }
    }
}