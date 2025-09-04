using UnityEngine;

public class BaseManager<T> : MonoBehaviour where T : BaseManager<T>
{
    private static T instance;
    public static T Instance // property public để lấy instance
    {
        get
        {
            if (instance == null) // nếu chưa có instance
            {
                instance = Object.FindFirstObjectByType<T>(); // tìm object loại T trong scene

                if (instance == null) // nếu vẫn chưa tìm thấy
                {
                    Debug.LogError($"No {typeof(T).Name} Singleton Instance."); // báo lỗi không có instance
                }
            }

            return instance; // trả về instance
        }
    }

    public static bool HasInstance // property kiểm tra có instance hay không
    {
        get
        {
            return (instance != null); // trả về true nếu instance tồn tại
        }
    }

    protected virtual void Awake() // Awake mặc định, có thể override
    {
        CheckInstance(); // gọi hàm kiểm tra instance
    }

    protected bool CheckInstance() // hàm kiểm tra và set instance
    {
        if (instance == null) // nếu chưa có instance
        {
            instance = (T)this; // gán instance là object hiện tại
            DontDestroyOnLoad(this); // giữ object không bị destroy khi load scene mới
            return true; // thành công
        }
        else if (instance == this) // nếu instance chính là object này
        {
            return true; // chấp nhận
        }

        Object.Destroy(this.gameObject); // nếu đã có instance khác → phá hủy object này
        return false; // thất bại
    }
}
