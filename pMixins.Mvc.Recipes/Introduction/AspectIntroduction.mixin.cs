//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by CopaceticSoftware.pMixins.CodeGenerator v0.1.12.328
//      for file D:\Projects\sandbox\pMixins\pMixins.Mvc.Recipes\Introduction\AspectIntroduction.cs.
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.  
// </auto-generated> 
//------------------------------------------------------------------------------
namespace pMixins.Mvc.Recipes.Introduction.Aspect
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
	public partial class AspectIntroduction : global::pMixins.AutoGenerated.pMixins.Mvc.Recipes.Introduction.Aspect.AspectIntroduction.pMixins.Mvc.Recipes.Introduction.HelloWorld.IHelloWorldRequirements, global::CopaceticSoftware.pMixins.ConversionOperators.IContainMixin<global::pMixins.Mvc.Recipes.Introduction.HelloWorld>
	{
		[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
		private sealed class __Mixins
		{
			public static global::System.Object ____Lock = new global::System.Object ();
			public readonly __pMixinAutoGenerated.pMixins_Mvc_Recipes_Introduction_HelloWorld.HelloWorldMasterWrapper pMixins_Mvc_Recipes_Introduction_HelloWorld;
			public __Mixins (AspectIntroduction host)
			{
				pMixins_Mvc_Recipes_Introduction_HelloWorld = global::CopaceticSoftware.pMixins.Infrastructure.MixinActivatorFactory.GetCurrentActivator ().CreateInstance<__pMixinAutoGenerated.pMixins_Mvc_Recipes_Introduction_HelloWorld.HelloWorldMasterWrapper> ((global::pMixins.AutoGenerated.pMixins.Mvc.Recipes.Introduction.Aspect.AspectIntroduction.pMixins.Mvc.Recipes.Introduction.HelloWorld.IHelloWorldRequirements)host);
			}
		}
		private __Mixins _____mixins;
		private __Mixins ___mixins {
			get {
				if (null == _____mixins) {
					lock (__Mixins.____Lock) {
						if (null == _____mixins) {
							_____mixins = new __Mixins (this);
						}
					}
				}
				return _____mixins;
			}
		}
		[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
		private sealed class __pMixinAutoGenerated
		{
			[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
			public sealed class pMixins_Mvc_Recipes_Introduction_HelloWorld
			{
				[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
				public abstract class HelloWorldProtectedMembersWrapper : global::pMixins.Mvc.Recipes.Introduction.HelloWorld
				{
					public HelloWorldProtectedMembersWrapper () : base ()
					{
					}
				}
				[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
				public class HelloWorldAbstractWrapper : HelloWorldProtectedMembersWrapper
				{
					private readonly global::pMixins.AutoGenerated.pMixins.Mvc.Recipes.Introduction.Aspect.AspectIntroduction.pMixins.Mvc.Recipes.Introduction.HelloWorld.IHelloWorldRequirements _target;
					public HelloWorldAbstractWrapper (global::pMixins.AutoGenerated.pMixins.Mvc.Recipes.Introduction.Aspect.AspectIntroduction.pMixins.Mvc.Recipes.Introduction.HelloWorld.IHelloWorldRequirements target)
					{
						_target = target;
					}
				}
				[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
				public sealed class HelloWorldMasterWrapper : global::CopaceticSoftware.pMixins.Infrastructure.MasterWrapperBase
				{
					public readonly global::pMixins.Mvc.Recipes.Introduction.HelloWorld _mixinInstance;
					public HelloWorldMasterWrapper (global::pMixins.AutoGenerated.pMixins.Mvc.Recipes.Introduction.Aspect.AspectIntroduction.pMixins.Mvc.Recipes.Introduction.HelloWorld.IHelloWorldRequirements mixinInstance)
					{
						_mixinInstance = base.TryActivateMixin<__pMixinAutoGenerated.pMixins_Mvc_Recipes_Introduction_HelloWorld.HelloWorldAbstractWrapper> (mixinInstance);
						base.Initialize (mixinInstance, _mixinInstance, new global::System.Collections.Generic.List<global::CopaceticSoftware.pMixins.Interceptors.IMixinInterceptor> {
							global::CopaceticSoftware.pMixins.Infrastructure.MixinActivatorFactory.GetCurrentActivator ().CreateInstance<global::pMixins.Mvc.Recipes.Introduction.Aspect.Aspect> ()
						});
					}
					[global::System.Diagnostics.DebuggerStepThrough]
					internal global::System.String SayHello ()
					{
						return base.ExecuteMethod ("SayHello", new global::System.Collections.Generic.List<global::CopaceticSoftware.pMixins.Interceptors.Parameter> {

						}, () => _mixinInstance.SayHello ());
					}
				}
			}
		}
		[global::System.Diagnostics.DebuggerStepThrough]
		public global::System.String SayHello ()
		{
			return ___mixins.pMixins_Mvc_Recipes_Introduction_HelloWorld.SayHello ();
		}
		[global::System.Diagnostics.DebuggerStepThrough]
		public static implicit operator global::pMixins.Mvc.Recipes.Introduction.HelloWorld (AspectIntroduction target) {
			return target.___mixins.pMixins_Mvc_Recipes_Introduction_HelloWorld._mixinInstance;
		}
		global::pMixins.Mvc.Recipes.Introduction.HelloWorld global::CopaceticSoftware.pMixins.ConversionOperators.IContainMixin<global::pMixins.Mvc.Recipes.Introduction.HelloWorld>.MixinInstance {
			get {
				return ___mixins.pMixins_Mvc_Recipes_Introduction_HelloWorld._mixinInstance;
			}
		}
	}
}
namespace pMixins.AutoGenerated.pMixins.Mvc.Recipes.Introduction.Aspect.AspectIntroduction.pMixins.Mvc.Recipes.Introduction.HelloWorld
{
	[System.CodeDom.Compiler.GeneratedCodeAttribute ("pMixin", "0.1.12.328")]
	public interface IHelloWorldRequirements
	{
	}
}
