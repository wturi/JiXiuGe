using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoShineMP.Models
{
    public class Scene
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsEnable { get; set; }

        public int MyProperty { get; set; }
    }
}