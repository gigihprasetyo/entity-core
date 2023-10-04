using System;
using System.Diagnostics.CodeAnalysis;

namespace qcs_product.API.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ListParameterNoteViewModel
    {
        public string Note { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime DateTime { get; set; }
    }
}
