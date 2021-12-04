using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RR_hookah.Models
{
    public class Category
    {
        /* в SQL сервере столбец может быть айди, те
         * значение этого столбца не нужно передавать в бд.
         * Каждый раз, когда добавляется новая запись,
         * его знач авто. увеличивается на 1 и создается новая запись.
         * Этот столбец должен быть и первичным ключом, а также id:
         */
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Порядок отображения категории должен быть больше нуля")]
        public int DisplayOrder { get; set; }
    }
}
