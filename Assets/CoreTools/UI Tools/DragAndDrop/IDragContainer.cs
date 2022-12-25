/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

namespace CoreTools.UI
{
    public interface IDragContainer<T> where T : class
    {
        T GetItem();
        void SetItem(T item, int amount);

        int GetAmount();
        void RemoveAmount(int amount);
        int TryAddAmount(T item, int amount);

        int MaxAcceptable(T item);
    }
}
