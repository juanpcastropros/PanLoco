using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PanLoco.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        #region AllowDecimalProperty
        /// <summary>
        /// Backing storage for the boolean flag which decides between
        /// integer vs. double validation.
        /// </summary>
        public static BindableProperty AllowDecimalProperty =
            BindableProperty.Create("AllowDecimal",
                typeof(bool), typeof(NumericValidationBehavior),
                true, BindingMode.OneWay);

        /// <summary>
        /// Bindable property to hold the boolean flag which decides
        /// whether we test for integer vs. double values.
        /// </summary>
        /// <value>The selected item.</value>
        public bool AllowDecimal
        {
            get { return (bool)base.GetValue(AllowDecimalProperty); }
            set { base.SetValue(AllowDecimalProperty, value); }
        }
        #endregion


        #region InvalidColorProperty
        /// <summary>
        /// Backing storage for the color used when the
        /// Entry has invalid data (non-numeric).
        /// </summary>
        //public static BindableProperty InvalidColorProperty =
        //    BindableProperty.Create("InvalidColor",
        //        typeof(Color), typeof(NumericValidationBehavior),
        //        Color.Red, BindingMode.OneWay);

        #endregion

        /// <summary>
        /// Called when this behavior is attached to a visual.
        /// </summary>
        /// <param name="bindable">Visual owner</param>
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        /// <summary>
        /// Called when this behavior is detached from a visual
        /// </summary>
        /// <param name="bindable">Visual owner</param>
        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        /// <summary>
        /// Called when the associated Entry has new text.
        /// This changes the text color to reflect whether the data
        /// is valid.
        /// </summary>
        /// <param name="sender">Entry control</param>
        /// <param name="args">TextChanged event arguments</param>
        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = Helpers.Const.Validation.IsNumeric(args.NewTextValue);
            //false;
            //if (AllowDecimal)
            //{
            //    double result;
            //    isValid = double.TryParse(args.NewTextValue, out result);
            //}
            //else
            //{
            //    long result;
            //    isValid = long.TryParse(args.NewTextValue, out result);
            //}

            ((Entry)sender).TextColor = isValid ? PanLoco.Helpers.Const.TextValidColor : PanLoco.Helpers.Const.TextInvalidColor;
        }
    }

}
