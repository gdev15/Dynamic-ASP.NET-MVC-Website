using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PortfolioWebsite.Models
{
    [Keyless] //There is no primary key cause there is no DB table so explicitly state it
    public class ContactViewModel
    {
        //We can use Data Annotation to Add validation to our model.
        //This is useful when we have required fields or need certain kinds of information.

        //? means property is nullable if we want to get rid of the green lines
        //public string Name? {get; set;}

        [Required(ErrorMessage = "*Name is required*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Email is required*")]
        [DataType(DataType.EmailAddress)] //Certain formatting is expected (@ symbol, .com, etc.)
        public string Email { get; set; }

        [Required(ErrorMessage = "*Subject is required*")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*Message is required*")]
        [DataType(DataType.MultilineText)] //MultilineText denotes this field is larger than a standard textbox (<input> => <textarea>)
        public string Message { get; set; }

        //MINI-LAB!
        //Create the ContactViewModel in your PersonalSite MVC Solution.
        //You can copy & paste teh properties & data annotations here
        //into that solution. Don't forget the using statement annotations
    }
}



