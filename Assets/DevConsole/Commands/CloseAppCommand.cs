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
    public class CloseAppCommand : ConsoleCommand
    {
        public override string Command => "closeapp";

        public override string WrongInputMessage => "This command doesn't take any inputs";

        public override string SuccessMessage => "Closed App.";

        public override bool Process(string[] args)
        {
            if (args.Length > 0)
                return false;
            UnityEngine.Application.Quit();
            return true;
        }
    }
}
