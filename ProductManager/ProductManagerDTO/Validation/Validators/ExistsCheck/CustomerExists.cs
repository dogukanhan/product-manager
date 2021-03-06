using ProductManager.IRepository;
using ProductManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace ProductManager.Validators
{
    /// <summary>
    /// Check if the Customer exists with given customer id
    /// </summary>
    public class CustomerExists : ValidationAttribute
    {
        /// <summary>
        /// Control if the entity exists with given primary key value.
        /// </summary>
        /// <param name="value">Primary key of the value</param>
        /// <param name="validationContext">ValidationContext from validation api.</param>
        /// <returns>ValidationResult that contains value is acceptable or not.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IUnitOfWork _unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));

            var resourceManager = (ResourceManager)validationContext.GetService(typeof(ResourceManager));

            if (value == null)
            {
                return new ValidationResult(string.Format(resourceManager.
                                                   GetString(nameof(ValidationMessageKey.Required)),
                                            validationContext.DisplayName
                                            ));
            }

            var customerId = (int)value;

            if (_unitOfWork.Repository<Customer>()
                            .Get(c => c.Id == customerId)
                            .GetAwaiter().GetResult()
                             != null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(string.Format(resourceManager.
                                                   GetString(nameof(ValidationMessageKey.NotExists)),
                                            validationContext.DisplayName
                                            ));
            }
        }
    }
}
