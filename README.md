# CSC 13001 - Lập trình ứng dụng Windows

---

# Báo cáo đồ án môn học - MILESTONE 01

## Thành viên nhóm

| STT | MSSV     | Họ tên           |
|-----|----------|------------------|
| 1   | 22120303 | Mai Xuân Quý     |
| 2   | 22120417 | Đỗ Thị Ánh Tuyết |
| 3   | 22120436 | Lê Cao Tuấn Vũ   |

## Mô tả về ứng dụng

- **Tên ứng dụng:** Clothing Store Manager
- **Mô tả:** Ứng dụng giúp quản lý bán hàng cho các shop thời trang nhỏ bao gồm các feature chính như quản lý nhân viên, bán lẻ hàng hóa, quản lý khách hàng, quản lý doanh số.

## Công nghệ sử dụng

- **Frontend**
  - WinUI 3 (Windows App SDK)
  - C# (.NET 7)
  - MVVM Pattern
  - Template Studio
- **Backend**
  - Spring boot (Java)
  - RESTful API
  - MySQL

## Những công việc đã hoàn thành

### Tóm tắt

- Backend:
  - Thiết kế database, kết nối database
  - Hoàn thành authentication
  - Hoàn thành thêm, xóa, sửa, phân trang, tìm kiếm, lưu ảnh lên server Cloudinary của product feature.
- Frontend:
  - Hoàn thành login, register, employee feature với mockdata
    - Login, register: đăng ký user mới, đăng nhập và logout
    - Employee: thêm, xóa, sửa employee, hiển thị phân trang
  - Xây dựng UI cho các trang còn lại

### Chi tiết

1. **UI/UX**
   - Giao diện được thiết kế sử dụng Template Studio với Navigation Bar, tối giản theo Windows 11 Design Principles.
   - Responsive theo các kích thước màn hình.
   - Hỗ trợ dark/light mode theo system.
   - Kiểm tra các lỗi người dùng nhập thiếu dữ liệu, sai định dạng dữ liệu.

2. **Kiến trúc**
   - MVVM Pattern: Pattern quan trọng nhất, là cấu trúc chính của dự án, tách biệt Bussiness Logic và UI. Các thành phần được xây dựng dựa trên pattern của MVVM Toolkit.

3. **Các tính năng nâng cao**
   - JWT Authentication (hiện tại đã được xây dựng xong ở phần backend, trong giai đoạn tiếp theo sau khi thay thế mockdata bằng RESTful API thì sẽ được áp dụng ở phần winui).
   - AutoSuggestBox: Tự động gợi ý và cập nhật thông tin hiển thị theo kết quả tìm kiếm, giúp người dùng tìm kiếm nhanh chóng và trực quan hơn (hiện tại chỉ khả dụng cho Tên).

4. **Đảm bảo chất lượng**
   - Tạo được các unit test để test các feature trước khi merge vào mã nguồn chính.
   - Ảnh minh chứng: [https://drive.google.com/drive/folders/1UBsc5dOgU-wsovWq1DChwhmf1aefPINo?usp=drive_link](https://drive.google.com/drive/folders/1UBsc5dOgU-wsovWq1DChwhmf1aefPINo?usp=drive_link)

5. **Sources**
   - Link repository: [vuhoabinhthachhoa/WindowProgramming](https://github.com/vuhoabinhthachhoa/WindowProgramming)
   - Video: [(16) WP Project - Milestone 1 - Demo - YouTube](https://www.youtube.com/watch?v=uBsfNTbp0LI)

## Team work

1. **Công cụ sử dụng**
   - Messenger, Notion, Jira

2. **Quy trình làm việc**
   - Team sẽ họp định kỳ vào mỗi 21h tối thứ 2 hàng tuần để báo cáo và phân chia công việc.
   - Các tài liệu về project như Database schema, features, workflow và các nguồn tài liệu sẽ được đăng trên một link Notion riêng của nhóm.
     - Notion link: [Project Docs]
   - Biên bản họp nhóm: [Google Docs](https://docs.google.com/document/d/1PCO1waWsLK8V03GiTuQv9KtMi7uyXxYTcoN9CwUKMBE/edit?usp=sharing)

3. **Làm việc với Git**
   - Các thành viên sẽ commit code lên một repository chung.
   - Sau khi các feature hoàn thành sẽ được chạy thử với test. Sau khi pass tests, thành viên sẽ tạo pull request để các thành viên khác vào review. Sau khi thống nhất, mã nguồn được merge vào branch main.
   - Link repository: [vuhoabinhthachhoa/WindowProgramming](https://github.com/vuhoabinhthachhoa/WindowProgramming)
