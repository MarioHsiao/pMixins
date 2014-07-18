﻿//----------------------------------------------------------------------- 
// <copyright file="NonPublicNonParamterlessConstructor.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Thursday, July 10, 2014 12:59:38 PM</date> 
// Licensed under the Apache License, Version 2.0,
// you may not use this file except in compliance with this License.
//  
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an 'AS IS' BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright> 
//-----------------------------------------------------------------------

using CopaceticSoftware.pMixins.Attributes;
using CopaceticSoftware.pMixins.Infrastructure;
using pMixins.AutoGenerated.pMixins.Mvc.Recipes.NonPublicNonParameterlessConstructor.NonPublicNonParameterlessConstructor.pMixins.Mvc.Recipes.NonPublicNonParameterlessConstructor.AbstractMixin;

namespace pMixins.Mvc.Recipes.NonPublicNonParameterlessConstructor
{
    public class Mixin
    {
        public Mixin()
        {
            FavoriteNumber = 42;
        }

        protected Mixin(int favoriteNumber)
        {
            FavoriteNumber = favoriteNumber;
        }

        public int FavoriteNumber { get; private set; }
    }

    public abstract class AbstractMixin
    {
        protected AbstractMixin(int favoriteNumber)
        {
            AbstractFavoriteNumber = favoriteNumber;
        }

        public int AbstractFavoriteNumber { get; private set; }
    }

    [pMixin(Mixin = typeof(Mixin), ExplicitlyInitializeMixin = true)]
    [pMixin(Mixin = typeof(AbstractMixin), ExplicitlyInitializeMixin = true)]
    public partial class NonPublicNonParameterlessConstructor
    {
        Mixin IMixinConstructorRequirement<Mixin>.InitializeMixin()
        {
            return new __pMixinAutoGenerated.pMixins_Mvc_Recipes_NonPublicNonParameterlessConstructor_Mixin.MixinAbstractWrapper(this, 1);
        }

        AbstractMixinAbstractWrapper IMixinConstructorRequirement<AbstractMixinAbstractWrapper>.InitializeMixin()
        {
            return new AbstractMixinAbstractWrapper(this, 1);
        }
    }
}
