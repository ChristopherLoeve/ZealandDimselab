using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZealandDimselab.Models
{
    public class Notifications
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  public int Id { get; set; }
        public List<string> StringNotifycations { get; set; }

        public Notifications(List<string> stringNotifycations)
        {
            StringNotifycations = stringNotifycations;
        }


    }
}
