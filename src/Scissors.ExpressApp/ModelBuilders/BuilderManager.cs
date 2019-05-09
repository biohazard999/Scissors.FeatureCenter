using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    public class BuilderManager : IBuilderManager
    {
        protected IList<IBuilder> Builders = new List<IBuilder>();

        public IBuilderManager AddBuilder(IBuilder builder)
        {
            Builders.Add(builder);
            return this;
        }

        public virtual void Build()
        {
            foreach(var builder in Builders)
            {
                BuildBuilder(builder);
            }
        }

        protected virtual void BuildBuilder(IBuilder builder)
            => builder.Build();
    }

    public interface IBuilderManager
    {
        IBuilderManager AddBuilder(IBuilder builder);

        void Build();
    }

    public interface IBuilder
    {
        void Build();
    }

    public interface ITypeInfoProvider
    {
        ITypeInfo TypeInfo { get; }
    }

    public interface ITypesInfoProvider
    {
        ITypesInfo TypesInfo { get; }
    }

    public class XafBuilderManager : BuilderManager, ITypesInfoProvider
    {
        public ITypesInfo TypesInfo { get; }

        public XafBuilderManager(ITypesInfo typesInfo)
            => TypesInfo = typesInfo;

        protected virtual void AddBuilders()
        {

        }

        public override void Build()
        {
            AddBuilders();
            base.Build();
        }

        protected override void BuildBuilder(IBuilder builder)
        {
            base.BuildBuilder(builder);
            if(builder is ITypeInfoProvider)
            {
                TypesInfo.RefreshInfo(((ITypeInfoProvider)builder).TypeInfo);                
            }
        }
    }
}
