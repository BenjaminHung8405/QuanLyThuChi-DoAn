using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QuanLyThuChi_DoAn.Data_Access_Layer.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        // Lấy toàn bộ dữ liệu
        IEnumerable<T> GetAll();

        // Lấy dữ liệu kèm theo điều kiện lọc (VD: Lọc theo TenantId)
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        // Tìm một bản ghi theo Khóa chính (Primary Key)
        T GetById(object id);

        // Thêm mới bản ghi
        void Add(T entity);

        // Cập nhật bản ghi
        void Update(T entity);

        // Xóa bản ghi theo ID
        void Delete(object id);

        // Lưu các thay đổi xuống Database
        int Save();
    }
}