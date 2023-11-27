using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.Model
{
    [Table("Orders")]
    public class Orders
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên người dùng không được để trống")]
        [Display(Name = "Tên người dùng")]
        public string UserID { get; set; }
        [Required(ErrorMessage = "Địa chỉ nhận hàng không được để trống")]
        [Display(Name = "Địa chỉ nhận hàng")]
        public string ReceiverAddress { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        [Required(ErrorMessage = "Người tạo không được để trống")]
        [Display(Name = "Người tạo")]
        public int CreateBy { get; set; }
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }
        [Display(Name = "Người cập nhật")]
        public int UpdateBy { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime UpdateAt { get; set; }
        [Display(Name = "Trạng thái")]
        public int? Status { get; set; }
    }
}
