﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildTasks.Utility
{
    /// <summary>
    /// Flexible Password Generator MSBuild Task
    /// </summary>
    public class PasswordGenerator : Task
    {
        /// <summary>
        /// Length of password to generate (default is 14 characters)
        /// </summary>
        public int PasswordLength { get; set; }

        /// <summary>
        /// Boolean (default is true) indicating whether alpha characters are to be used when building a password string
        /// </summary>
        public bool IncludeAlpha { get; set; }

        /// <summary>
        /// Optional input defines which alpha characters are to be used when building a password string
        /// </summary>
        public string AlphaSet { get; set; }

        /// <summary>
        /// Boolean (default is true) indiciating whether numeric characters are to be used when building a password string
        /// </summary>
        public bool IncludeNumeric { get; set; }

        /// <summary>
        /// Optional input defines which numbers are to be used when building a password string
        /// </summary>
        public string NumericSet { get; set; }

        /// <summary>
        /// Boolean (default is true) indiciates whether special characters are to be used when building a password string
        /// </summary>
        public bool IncludeSpecialCharacters { get; set; }

        /// <summary>
        /// Optional input defines which special characters are to be used when building a password string
        /// </summary>
        public string SpecialCharacterSet { get; set; }

        /// <summary>
        /// Setting DebugMode to true will output the value of the password to the console (Default is false)
        /// </summary>
        public bool DebugMode { get; set; }

        /// <summary>
        /// Password string generated by process to be output to MSBuild
        /// </summary>
        [Output]
        public string Password { get; set; }

        /// <summary>
        /// Constructor sets default values for inputs that are needed but not required via MSBuild
        /// </summary>
        public PasswordGenerator()
        {
            this.PasswordLength = 14;
            this.IncludeAlpha = true;
            this.AlphaSet = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            this.IncludeNumeric = true;
            this.NumericSet = "0987654321";
            this.IncludeSpecialCharacters = true;
            this.SpecialCharacterSet = "!@$?_-";
            this.DebugMode = false;
        }
        
        /// <summary>
        /// Override of Task.Execute to generate password and return to MSBuild via an output parameter
        /// </summary>
        /// <returns>true if password was generated, false if not</returns>
        public override bool Execute()
        {
            Log.LogMessage("  PasswordGenerator.Execute beginning process");
            bool retVal = false;
            string validCharacters = GetAllowedCharacters();
            char[] passwordChars = new char[this.PasswordLength];
            Random rand = new Random();

            for (int i = 0; i < this.PasswordLength; i++)
            {
                passwordChars[i] = validCharacters[rand.Next(0, validCharacters.Length)];
            }

            this.Password = new string(passwordChars);

            if (!string.IsNullOrWhiteSpace(this.Password))
            {
                retVal = true;

                Log.LogMessage("  Password Successfully Generated");

                if (this.DebugMode)
                {
                    Log.LogMessage("    Password Value is: " + this.Password);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Private method gets all valid characters for builing a connection string based on optional inputs or defaults
        /// </summary>
        /// <returns>string of characters to be used as valid characters in building a password</returns>
        private string GetAllowedCharacters()
        {
            string retVal = string.Empty;

            if (this.IncludeAlpha)
            {
                retVal += this.AlphaSet;
            }
            if (this.IncludeNumeric)
            {
                retVal += this.NumericSet;
            }
            if (this.IncludeSpecialCharacters)
            {
                retVal += this.SpecialCharacterSet;
            }

            return retVal;
        }
    }
}