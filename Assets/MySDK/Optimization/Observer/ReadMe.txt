Sử dụng để thông báo sự kiện giữa các class với nhau, từ đó giảm sự phụ thuộc giữa các
class

File EventKey sẽ chứa các loại sự kiện (giống id để phân biệt)

// Cách dùng cơ bản
1. Đăng kí sự kiện không tham số
Messenger.AddListener(EventKey.OnStartGame, OnInit);
(OnInit là hàm không tham số được gọi mỗi khi kích hoạt sự kiện OnStartGame)

2. Đăng kí sự kiện có tham số
Messenger.AddListener<int>(EventKey.OnStartGame, OnInit);
(OnInit là hàm có tham số int được gọi mỗi khi kích hoạt sự kiện OnStartGame)
    - nhiếu tham số thì sẽ phân tách bằng dấu phẩy VD <int, bool, class...> ( max 5 )
    - khuyến kích gom lại thành 1 class chứa tập các tham số thay vì truyền nhiều

3. Hủy sự kiện không tham số (note tương tự như 1)
Messenger.RemoveListener(EventKey.OnStartGame, OnInit);

4. Hủy sự kiện có tham số (note tương tự như 2)
Messenger.RemoveListener<int>(EventKey.OnStartGame, OnInit);

5. Gửi sự kiện, khi gọi hàm này tất cả các class được đăng kí sẽ tự động thực thi hàm đã khai báo
    - không tham số: Messenger.Broadcast(EventKey.OnStartGame);
    - có tham số: Messenger.Broadcast(EventKey.OnStartGame, 1);
    
Lưu ý:
- Khai báo loại sự kiện cần thông báo giữa các class trong EventKey
- Các đăng kí thường sẽ được gọi trong Awake hoặc OnEnable tùy nhu cầu dùng
- Các hủy sự kiện thường sẽ được gọi trong OnDestroy hoặc OnDisable tùy nhu cầu dùng
- Mỗi một EventKey chỉ có 1 loại sự kiện đăng kí (có tham số or không tham số) không được trùng nhau
- Đây là kiến trúc event driven nên hiệu suất sẽ k tốt lắm, nhưng tầm cả 100 sự kiện vẫn thoải mái
