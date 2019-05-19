using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.DC;

namespace Scissors.ExpressApp.ModelBuilders
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Scissors.ExpressApp.ModelBuilders.BuilderManager" />
    /// <seealso cref="Scissors.ExpressApp.ModelBuilders.ITypesInfoProvider" />
    public class XafBuilderManager : BuilderManager, ITypesInfoProvider
    {
        /// <summary>
        /// Gets the empty builders.
        /// </summary>
        /// <value>
        /// The empty builders.
        /// </value>
        public static IEnumerable<IBuilder> EmptyBuilders { get; } = new IBuilder[0];
        
        /// <summary>
        /// Gets the types information.
        /// </summary>
        /// <value>
        /// The types information.
        /// </value>
        public ITypesInfo TypesInfo { get; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="XafBuilderManager"/> class.
        /// </summary>
        /// <param name="typesInfo">The types information.</param>
        public XafBuilderManager(ITypesInfo typesInfo) : this(typesInfo, EmptyBuilders) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XafBuilderManager"/> class.
        /// </summary>
        /// <param name="typesInfo">The types information.</param>
        /// <param name="builders">The builders.</param>
        public XafBuilderManager(ITypesInfo typesInfo, IEnumerable<IBuilder> builders) :base()
        {
            TypesInfo = typesInfo;
            AddBuilders(builders);
        }

        /// <summary>
        /// Creates the specified types information.
        /// </summary>
        /// <param name="typesInfo">The types information.</param>
        /// <param name="builders">The builders.</param>
        /// <returns></returns>
        public static XafBuilderManager Create(ITypesInfo typesInfo, IEnumerable<IBuilder> builders)
            => new XafBuilderManager(typesInfo, builders);

        /// <summary>
        /// Builds the builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
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
