using Gender.GrpcService.DuyVK.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gender.GrpcClient.DuyVK
{
    public static class Helpers
    {
        /**
         * Prompt for string
         */
        public static string Prompt(string label)
        {
            Console.Write($"{label}: ");
            return Console.ReadLine().Trim() ?? "";
        }

        /**
         * Prompt for int
         */
        public static int? PromptInt(string label)
        {
            Console.Write($"{label}: ");
            return int.TryParse(Console.ReadLine(), out var i) ? i : null;
        }

        /**
         * Prompt for double
         */
        public static double? PromptDouble(string label)
        {
            Console.Write($"{label}: ");
            return double.TryParse(Console.ReadLine(), out var i) ? i : null;
        }

        /**
         * Print reminder list
         */
        public static void PrintReminderList(ICollection<MenstrualCycleReminderDuyVK> list)
        {
            Console.WriteLine(
                "\n\n\t{0,-4} | {1,-3} | {2,-10} | {3,-20} | {4,-19} | {5,-19} | {6,-10} | {7,-10} | {8,-5} | {9,-6} | {10,-8}",
                "ID",
                "Cat",
                "Cat.Code",
                "CatName",
                "Title",
                "Note",
                "Date",
                "SentAt",
                "Sent",
                "Repeat",
                "Scpre"
            );
            foreach (var r in list)
            {
                Console.WriteLine(
                    "\t{0,-4} | {1,-3} | {2,-10} | {3,-20} | {4,-19} | {5,-19} | {6,-10:yyyy-MM-dd} | {7,-10:yyyy-MM-dd} | {8,-5} | {9,-6} | {10,-6}",
                    r.MenstrualCycleReminderDuyVKid,
                    r.ReminderCategoryDuyVKid,
                    r.ReminderCategoryDuyVK.Code,
                    r.ReminderCategoryDuyVK.Name,
                    // Truncate long text to fit
                    r.Title.Length <= 19 ? r.Title : r.Title[..16] + "...",
                    r.Note.Length <= 19 ? r.Note : r.Note[..16] + "...",
                    DateTime.Parse(r.ReminderDate),
                    string.IsNullOrEmpty(r.SentAt) ? "-" : DateTime.Parse(r.SentAt),
                    r.IsSent,
                    r.RepeatInterval,
                    r.ImportanceScore
                );
            }
        }

        /**
         * Print category list
         */
        public static void PrintCategoryList(ICollection<ReminderCategoryDuyVK> list)
        {
            Console.WriteLine(
                "\n\n\t{0,-4} | {1,-10} | {2,-20} | {3,-25} | {4,-6} | {5,-8} | {6,-6} | {7}",
                "ID", "Code", "Name", "Description", "Active", "Priority", "Offset", "Color"
            );

            foreach (var c in list)
            {
                Console.WriteLine(
                    "\t{0,-4} | {1,-10} | {2,-20} | {3,-25} | {4,-6} | {5,-8} | {6,-6} | {7}",
                    c.ReminderCategoryDuyVKid,
                    c.Code.Length <= 10 ? c.Code : c.Code[..7] + "...",
                    c.Name.Length <= 20 ? c.Name : c.Name[..17] + "...",
                    c.Description.Length <= 25 ? c.Description : c.Description[..22] + "...",
                    c.IsActive.ToString() ?? "—",
                    c.PriorityLevel.ToString() ?? "—",
                    c.DefaultOffset.ToString() ?? "—",
                    c.ColorCode
                );
            }
        }
    }
}
