using System.Diagnostics.Metrics;
using System.Security.Cryptography;

namespace MultiValue
{
    public class Program
    {
        static GenericDictionaryExtn<string, string> dictionary = new GenericDictionaryExtn<string, string>();    //Create object of generic class
        enum Command    //enum for commands
        {
            KEYS,
            MEMBERS,
            ADD,
            REMOVE,
            REMOVEALL,
            CLEAR,
            KEYEXISTS,
            MEMBEREXISTS,
            ALLMEMBERS,
            ITEMS,
            COMMANDS,
            HELP,
            EXIT
        }
        public static void Main(string[] args)
        {
            ListCommands();
            bool isRunning = true;
            while (isRunning)
            {
                Console.Write("\tEnter a command : ");
                string? inputCmd = Console.ReadLine();
                if (!string.IsNullOrEmpty(inputCmd))
                {
                    int itemCount = 1;
                    string[] inputArgs = inputCmd.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);  //handle whitespaces split into array of characters that delimit the substrings and using the split option remove empty entries
                    string argCmd = inputArgs[0].ToString();
                    string argKey = inputArgs.Count() > 1 ? inputArgs[1].ToString() : string.Empty;
                    string argMember = inputArgs.Count() == 3 ? inputArgs[2].ToString() : string.Empty;
                    switch (argCmd.ToUpper())
                    {
                        case nameof(Command.KEYS):  //Nameof is evaluated at compile time, simply to a string that matches the (unqualified) name of the given variable, type, or member.

                            if (inputArgs.Count() != 1)
                            {
                                syntaxError();
                            }
                            else
                            {
                                var keys = dictionary.GetAllKeys();
                                if (keys.Count() > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    foreach (var key in keys)
                                    {
                                        Console.WriteLine("{0}{1}{2}{3}", "\t", itemCount, ") ", key);
                                        itemCount++;
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tNo Keys in Dictionary");
                                }
                            }
                            break;
                        case nameof(Command.MEMBERS):
                            if (inputArgs.Count() != 2)
                            {
                                syntaxError();
                            }
                            else
                            {
                                if (dictionary.KeyExists(argKey))
                                {
                                    var members = dictionary.GetMemberByKey(argKey);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    itemCount = 1;
                                    foreach (var member in members)
                                    {
                                        Console.WriteLine("{0}{1}{2}{3}", "\t", itemCount, ") ", member);
                                        itemCount++;
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tError, key does not exist");
                                }
                            }
                            break;
                        case nameof(Command.ADD):
                            if (inputArgs.Count() != 3)
                            {
                                syntaxError();
                            }
                            else
                            {
                                if (dictionary.MemberExists(argKey, argMember))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tERROR, member already exists for key");
                                }
                                else
                                {
                                    dictionary.Add(argKey, argMember);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\tAdded");
                                }
                            }
                            break;
                        case nameof(Command.REMOVE):
                            if (inputArgs.Count() != 3)
                            {
                                syntaxError();
                            }
                            else
                            {
                                if (dictionary.KeyExists(argKey))
                                {
                                    bool status = dictionary.Remove(argKey, argMember); //define status code
                                    if (status)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("\tMember Removed");
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("\tError, member does not exist");
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tError, key does not exist");
                                }
                            }
                            break;
                        case nameof(Command.REMOVEALL):
                            if (inputArgs.Count() != 2)
                            {
                                syntaxError();
                            }
                            else
                            {
                                if (dictionary.KeyExists(argKey))
                                {
                                    dictionary.RemoveAll(argKey);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\tKey and its member removed");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tError,Key does not exist");
                                }
                            }
                            break;
                        case nameof(Command.CLEAR):
                            if (inputArgs.Count() != 1)
                            {
                                syntaxError();
                            }
                            else
                            {
                                bool isCleared = dictionary.Clear();
                                if (isCleared)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\tCleared");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tDictionary is empty");
                                }
                            }
                            break;
                        case nameof(Command.KEYEXISTS):
                            if (inputArgs.Count() != 2)
                            {
                                syntaxError();
                            }
                            else
                            {
                                if (dictionary.KeyExists(argKey))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\tTrue");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tFalse");
                                }
                            }
                            break;
                        case nameof(Command.MEMBEREXISTS):
                            if (inputArgs.Count() != 3)
                            {
                                syntaxError();
                            }
                            else
                            {
                                if (dictionary.MemberExists(argKey, argMember))
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\tTrue");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tFalse");
                                }
                            }
                            break;
                        case nameof(Command.ALLMEMBERS):
                            if (inputArgs.Count() != 1)
                            {
                                syntaxError();
                            }
                            else
                            {
                                var all_member = dictionary.AllMembers();
                                if (all_member.Count == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tNo Members in Dictionary");
                                }
                                else
                                {
                                    itemCount = 1;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    foreach (var member in all_member)
                                    {                                        
                                        Console.WriteLine("{0}{1}{2}{3}", "\t", itemCount, ") ", member);
                                        itemCount++;
                                    }
                                }
                            }
                            break;
                        case nameof(Command.ITEMS):
                            if (inputArgs.Count() != 1)
                            {
                                syntaxError();
                            }
                            else
                            {

                                var items = dictionary.GetItems();
                                if (items.Count > 0)
                                {
                                    itemCount = 1;
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    foreach (var item in items.Keys)
                                    {
                                        foreach (var item2 in items[item])
                                        {                                            
                                            Console.WriteLine("{0}{1}{2}{3}", "\t", itemCount, ") ", item.ToString() + ": " + item2.ToString());
                                            itemCount++;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\tNo items in dictionary to display");
                                }
                            }
                            break;
                        case nameof(Command.COMMANDS):
                            ListCommands();
                            break;
                        case nameof(Command.HELP):
                            if (inputArgs.Count() != 1)
                            {
                                syntaxError();
                            }
                            else
                            {
                                Help();
                            }
                            break;
                        case nameof(Command.EXIT):
                            isRunning = false;
                            break;
                        default:
                            syntaxError();
                            break;
                    }
                    Console.ResetColor();
                }
            }
            static void syntaxError()
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\tPlease check the command, type \"Help\" command to see details");
                Console.ResetColor();
            }
            static void Help()
            {
                string str = new string('-', 20);
                Console.WriteLine("\t" + str);
                Console.WriteLine("\tCommand Line Help");
                str = new string('-', 20);
                Console.WriteLine("\t" + str);
                Console.WriteLine("\tADD <key name> <member name> : To add key and member, new keys will be added automatically along with member if key does not exist.");
                Console.WriteLine("\tMEMBERS <key name> : To view members of a key.");
                Console.WriteLine("\tKEYS: To view all the keys type the command KEYS (without any arguments).");
                Console.WriteLine("\tREMOVE <key name> <member name> : To remove member of a key");
                Console.WriteLine("\tREMOVEALL <key name> : To remove a key and all member of a key.");
                Console.WriteLine("\tCLEAR : Clears the dictionary, all keys and its member will be removed.");
                Console.WriteLine("\tKEYEXISTS <key name> : To check if key exist in the dictionary.");
                Console.WriteLine("\tMEMBEREXISTS <key name> <member name> : To check if member exists within a key.");
                Console.WriteLine("\tALLMEMBERS: To view all members in the dictionary of all keys");
                Console.WriteLine("\tITEMS: To view all keys and their members in the dictionary type ITEMS");
                str = new string('-', 80);
                Console.WriteLine("\t" + str);

            }
            static void ListCommands()
            {
                Console.WriteLine("\tDictionary Commands");
                string str = new string('-', 20);
                Console.WriteLine("\t"+str);
                foreach (var name in Enum.GetNames<Command>())
                {
                    Console.WriteLine("\t" + name.ToString());
                }
            }

        }
    }
}