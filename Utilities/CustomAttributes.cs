using System;
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner {
    public class InTheFuture : ValidationAttribute {
        private DateTime CurrentDate;

        public InTheFuture () {
            CurrentDate = DateTime.Now;
        }

        protected override ValidationResult IsValid (object value, ValidationContext validationContext) {
            DateTime setVal = (DateTime) value;
            if (setVal > CurrentDate) {
                return ValidationResult.Success;
            }
            return new ValidationResult ("Wedding date must be in the future");
        }
    }
}

