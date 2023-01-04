/* Copyright (c) 2022 - Christoph R�mer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

namespace CoreTools.Console
{
    public interface IConsoleCommand
    {
        string Command { get; }
        bool Process(string[] args);
        string WrongInputMessage { get; }
        string SuccessMessage { get; }
    }
}