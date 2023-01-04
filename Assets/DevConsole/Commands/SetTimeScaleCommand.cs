/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

namespace CoreTools.Console.Commands
{
    public class SetTimeScaleCommand : ConsoleCommand
    {
        public override string Command => "settimescale";

        public override string WrongInputMessage => "This command takes one argument in form of a float value.";

        const string successMessageBody = "TimeScale has been set to ";
        private string successMessage = string.Empty;
        public override string SuccessMessage => successMessage;

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
                return false;

            if (float.TryParse(args[0], out float result))
            {
                successMessage = successMessageBody + result.ToString();
                UnityEngine.Time.timeScale = result;
            }
            else return false;

            return true;
        }
    }
}
