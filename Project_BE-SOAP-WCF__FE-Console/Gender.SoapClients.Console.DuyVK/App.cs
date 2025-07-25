using Gender.SoapClients.AppConsole.DuyVK.TokenHandlers;
using MenstrualCycleReminderDuyVKServiceReference;
using ReminderCategoryDuyVKServiceReference;
using System.ServiceModel;
using System.Text;
using SystemUserAccountDuyVKServiceReference;

namespace Gender.SoapClients.AppConsole.DuyVK
{
    public class App
    {
        // ==========================
        // === Fields
        // ==========================

        private string _jwtToken = string.Empty;
        private MenstrualCycleReminderDuyVKSoapServiceClient _reminderClient;
        private ReminderCategoryDuyVKSoapServiceClient _categoryClient;
        private SystemUserAccountSoapServiceClient _authClient;

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
                        case "2": await ListPagedReminders(); break;
                        case "3": await SearchReminders(); break;
                        case "4": await GetReminderById(); break;
                        case "5": await CreateReminder(); break;
                        case "6": await UpdateReminder(); break;
                        case "7": await DeleteReminder(); break;
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
         * Initialize Service Clients
         */
        public void Initialize()
        {
            _authClient = new SystemUserAccountSoapServiceClient();
            _categoryClient = new ReminderCategoryDuyVKSoapServiceClient();
            _reminderClient = new MenstrualCycleReminderDuyVKSoapServiceClient();

            // Add token guard to the clients
            var behavior = new BearerTokenBehavior(() => _jwtToken);
            _reminderClient.Endpoint.EndpointBehaviors.Add(behavior);
            _categoryClient.Endpoint.EndpointBehaviors.Add(behavior);
        }

        /**
         * Loop for prompting user for login credentials
         */
        private async Task LoginLoopAsync()
        {
            while (true)
            {
                Console.Write("Username: ");
                var user = Console.ReadLine()!.Trim();
                Console.Write("Password: ");
                var pass = ReadPassword();

                try
                {
                    // SOAP login returns LoginResponse { Token, ExpiresIn }
                    var rsp = await _authClient.LoginAsync(new LoginRequest
                    {
                        UserName = user,
                        Password = pass
                    });

                    _jwtToken = rsp.Token;
                    Console.WriteLine($"\n\tLogged in! Token valid for {rsp.ExpiresIn}s\n");
                    return;
                }
                catch (FaultException ex)
                {
                    Console.WriteLine("\n\tInvalid credentials, try again.\n");
                }
            }
        }

        /**
         * TODO!
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
                Console.WriteLine("\n\t-- Categories --");
                var resp = await _categoryClient.GetAllAsync();
                Helpers.PrintCategoryList(resp);
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine($"Communication error: {ce.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // ==========================
        // === Reminders
        // ==========================

        /**
         * Get a reminder by ID
         */
        public async Task GetReminderById()
        {
            try
            {
                Console.Write("\nEnter reminder ID: ");
                if (!int.TryParse(Console.ReadLine(), out var id))
                {
                    Console.WriteLine("Invalid ID.");
                    return;
                }

                var singleResult = await _reminderClient.GetByIdAsync(id);
                Helpers.PrintReminderList(new[] { singleResult });
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /**
         * List all reminders
         */
        public async Task ListAllReminders()
        {
            try
            {
                Console.WriteLine("\n\t-- All Reminders --\n");

                var all = await _reminderClient.GetAllAsync();
                Helpers.PrintReminderList(all);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /**
         * Search reminders by title, importance score, and color code
         */
        public async Task SearchReminders()
        {
            try
            {
                int page = Helpers.PromptInt("Page number (default is 1)") ?? 1;
                int size = Helpers.PromptInt("Page size (default is 5)") ?? 5;
                string title = Helpers.Prompt("Title (optional, leave empty for all)");
                double? score = Helpers.PromptDouble("Important Score (optional, leave empty for all)");
                string colorCode = Helpers.Prompt("Color Code (optional, leave empty for all)");

                var req = new MenstrualCycleReminderSearchRequest
                {
                    CurrentPage = page,
                    PageSize = size,
                    Title = title,
                    ColorCode = colorCode,
                    ImportanceScore = score
                };
                var result = await _reminderClient.SearchAsync(req);
                Console.WriteLine($"\nPage {result.CurrentPage}/{result.TotalPages} ({result.TotalItems} total)\n");
                Helpers.PrintReminderList(result.Items);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /**
         * List reminders with pagination
         * Input: page number and page size
         */
        public async Task ListPagedReminders()
        {
            try
            {
                int page = Helpers.PromptInt("Page number (default is 1)") ?? 1;
                int size = Helpers.PromptInt("Page size (default is 5)") ?? 5;

                var req = new SearchRequest { CurrentPage = page, PageSize = size };
                var paged = await _reminderClient.GetAllPagedAsync(req);

                Console.WriteLine($"\nPage {paged.CurrentPage}/{paged.TotalPages} ({paged.TotalItems} total)\n");
                Helpers.PrintReminderList(paged.Items);
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }


        /**
         * Create New Reminder
         */
        public async Task CreateReminder()
        {
            try
            {
                await ListAllCategories();
                Console.WriteLine("\n\t-- Create New Reminder --\n");

                var model = new MenstrualCycleReminderDuyVK
                {
                    ReminderCategoryDuyVKid = Helpers.PromptInt("Category ID") ?? 0,
                    Title = Helpers.Prompt("Title"),
                    Note = Helpers.Prompt("Note"),
                    ReminderDate = DateTime.Parse(Helpers.Prompt("Reminder date (yyyy-MM-dd)")),
                    SentAt = DateTime.Parse(Helpers.Prompt("Sent at (yyyy-MM-dd or empty)")),
                    IsSent = bool.TryParse(Helpers.Prompt("Is sent (true/false)"), out var f) && f,
                    RepeatInterval = Helpers.PromptInt("Repeat interval") ?? 0,
                    ImportanceScore = double.TryParse(Helpers.Prompt("Importance score"), out var d) ? d : 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var newId = await _reminderClient.CreateAsync(model);
                Console.WriteLine($"\nCreated with ID: {newId}");
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /**
         * Update Reminders
         */
        public async Task UpdateReminder()
        {
            try
            {
                await ListAllReminders();

                Console.WriteLine("\n\t-- Update Reminder --\n");

                // prompt id
                var id = Helpers.PromptInt("ID to update");

                if (id == null || id <= 0)
                    throw new InvalidDataException("Pleae enter id greater than 0. Please try again");

                var model = new MenstrualCycleReminderDuyVK
                {
                    MenstrualCycleReminderDuyVKid = id.Value,
                    ReminderCategoryDuyVKid = Helpers.PromptInt("Category ID") ?? 0,
                    Title = Helpers.Prompt("Title"),
                    Note = Helpers.Prompt("Note"),
                    ReminderDate = DateTime.Parse(Helpers.Prompt("Reminder date (yyyy-MM-dd)")),
                    SentAt = DateTime.Parse(Helpers.Prompt("Sent at (yyyy-MM-dd or empty)")),
                    IsSent = bool.TryParse(Helpers.Prompt("Is sent (true/false)"), out var f) && f,
                    RepeatInterval = Helpers.PromptInt("Repeat interval") ?? 0,
                    ImportanceScore = double.TryParse(Helpers.Prompt("Importance score"), out var d) ? d : 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var updatedCount = await _reminderClient.UpdateAsync(model);
                Console.WriteLine($"\nUpdated {updatedCount} record(s).");
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /**
         * Delete By Id
         */
        public async Task DeleteReminder()
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

                var deletedCount = await _reminderClient.DeleteAsync(id);
                Console.WriteLine($"\nDeleted {deletedCount} record(s).");
            }
            catch (FaultException fe) when (fe.Message.Contains("Missing Bearer"))
            {
                Console.WriteLine("Not authorized — please log in.");
            }
            catch (FaultException fe)
            {
                Console.WriteLine($"SOAP error: {fe.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}