using System;

namespace Assets.IUnified.Sample
{
    public interface IMyInterface
    {
        string StringProperty { get; }

        void Method();
    }

    [Serializable]
    public class MyInterfaceContainer : IUnifiedContainer<IMyInterface> { }
}