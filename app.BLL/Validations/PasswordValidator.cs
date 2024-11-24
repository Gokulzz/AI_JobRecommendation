﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.BLL.DTO;
using FluentValidation;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace app.BLL.Validations
{
    public class PasswordValidator : AbstractValidator<PasswordResetDTO>
    {
        public PasswordValidator()
        {
            RuleFor(c => c.Password).NotEmpty().MinimumLength(8)
                    .Matches(@"[A-Z]+").WithMessage("Your password should contain at least one Uppercase characters")
                    .Matches(@"[a-z]+").WithMessage("Your password should containt at least one Lowercase characters")
                    .Matches(@"[0-9]+").WithMessage("Your password should containt at least one  number")
                    .Matches(@"[\!\@\#\$]+").WithMessage("Your password should contain at least one special character" + "" +
                    "! @ # $");
            RuleFor(c => c.ConfirmPassword).NotEmpty().Matches(c => c.Password);
        }
    }
}