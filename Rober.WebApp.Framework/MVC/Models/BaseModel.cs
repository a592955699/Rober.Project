﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Rober.WebApp.Framework.MVC.Models
{
    /// <summary>
    /// Represents base nopCommerce model
    /// </summary>
    public partial class BaseModel
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public BaseModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
            PostInitialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Perform additional actions for binding the model
        /// </summary>
        /// <param name="bindingContext">Model binding context</param>
        /// <remarks>Developers can override this method in custom partial classes in order to add some custom model binding</remarks>
        public virtual void BindModel(ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Perform additional actions for the model initialization
        /// </summary>
        /// <remarks>Developers can override this method in custom partial classes in order to add some custom initialization code to constructors</remarks>
        protected virtual void PostInitialize()
        {            
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets property to store any custom values for models 
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }

        #endregion

    }

    /// <summary>
    /// Represents base nopCommerce entity model
    /// </summary>
    public partial class BaseEntityModel : BaseEntityModel<int>
    {
    }

    /// <summary>
    /// Represents base nopCommerce entity model
    /// </summary>
    public partial class BaseEntityModel<TKeyType> : BaseModel 
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual TKeyType Id { get; set; }
    }
}
