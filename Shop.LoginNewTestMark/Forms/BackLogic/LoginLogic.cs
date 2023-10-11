﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.LoginNewTestMark.Forms.BackLogic
{
    internal abstract class LoginLogic
    {
        protected const string FileName = "UserData.json";

        public static bool TryLogin(string firstName, string password)
        {
            try
            {
                if (!File.Exists(FileName))
                {
                    throw new FileNotFoundException($"File {FileName} not found.");
                }

                string jsonFromFile = File.ReadAllText(FileName);
                List<RegistrationLogic> userList = JsonConvert.DeserializeObject<List<RegistrationLogic>>(jsonFromFile);

                RegistrationLogic user = userList.Find(u => u.FirstName == firstName && u.Password == password);

                return user != null;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool TryLogin(string firstName)
        {
            return TryLogin(firstName, "");
        }

        public abstract string GetNewPassword(string password);

        public abstract string AdditionalProperty { get; }
    }
}
