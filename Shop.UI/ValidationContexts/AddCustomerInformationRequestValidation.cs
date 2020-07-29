using FluentValidation;
using Shop.Application.Cart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ValidationContexts
{
    public class AddCustomerInformationRequestValidation 
        : AbstractValidator<AddCustomerInformation.Request>
    {
        public AddCustomerInformationRequestValidation()
        {
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotNull().MinimumLength(7);
            RuleFor(x => x.Address1).NotNull();
            RuleFor(x => x.City).NotNull();
            RuleFor(x => x.PostCode).NotNull();
        }
    }
}
