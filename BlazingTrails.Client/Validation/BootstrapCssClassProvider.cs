using Microsoft.AspNetCore.Components.Forms;

namespace BlazingTrails.Client
{
    public class BootstrapCssClassProvider:FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if (editContext.IsModified(fieldIdentifier))
            {
                return isValid ? "is-valid" : "is-invalid";
            }

            Span<int> storage = stackalloc int[10];
            int num = 0;
            foreach (ref int item in storage)
            {
                item = num++;
            }
            foreach (ref readonly var item in storage)
            {
                Console.Write($"{item} ");
            }

            return isValid ? "" : "is-invalid";

        }



    }
}
