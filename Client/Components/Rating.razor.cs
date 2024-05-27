using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace Reflectionnaire.Client.Components
{
    /// <summary>
    /// Clone of RadzenRating to remove clear-button (will create PR for Radzen)
    /// </summary>
    public partial class Rating : FormComponent<int>
    {
        protected override string GetComponentCssClass()
        {
            return GetClassList("rz-rating").ToString();
        }

        /// <summary>
        /// Gets or sets the number of stars.
        /// </summary>
        /// <value>The number of stars.</value>
        [Parameter]
        public int Stars { get; set; } = 5;

        /// <summary>
        /// Gets or sets the clear aria label text.
        /// </summary>
        /// <value>The clear aria label text.</value>
        [Parameter]
        public string ClearAriaLabel { get; set; } = "Clear";

        /// <summary>
        /// Gets or sets the rate aria label text.
        /// </summary>
        /// <value>The rate aria label text.</value>
        [Parameter]
        public string RateAriaLabel { get; set; } = "Rate";

        private async System.Threading.Tasks.Task SetValue(int value)
        {
            if (!Disabled)
            {
                Value = value;

                await ValueChanged.InvokeAsync(value);
                if (FieldIdentifier.FieldName != null) { EditContext?.NotifyFieldChanged(FieldIdentifier); }
                await Change.InvokeAsync(value);
            }
        }

        bool preventKeyPress = true;
        async Task OnKeyPress(KeyboardEventArgs args, Task task)
        {
            var key = args.Code != null ? args.Code : args.Key;

            if (key == "Space" || key == "Enter")
            {
                preventKeyPress = true;

                await task;
            }
            else
            {
                preventKeyPress = false;
            }
        }
    }
}