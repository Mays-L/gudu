using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject); // برای حفظ منیجر در طول بازی
    }

    // ذخیره جهت جدید و زمان انتخاب آن برای هر چهارراه
    public void SaveDirection(int crossroadIndex, int direction, float timeSpent)
    {
        // ذخیره جهت انتخاب شده
        PlayerPrefs.SetInt("Direction_Crossroad_" + crossroadIndex, direction);

        // ذخیره زمان انتخاب جهت
        PlayerPrefs.SetFloat("DirectionTime_Crossroad_" + crossroadIndex, timeSpent);

        // ذخیره تغییرات
        PlayerPrefs.Save();
    }

    // متد برای بارگذاری جهت‌ها و زمان‌ها
    public int LoadDirection(int crossroadIndex)
    {
        return PlayerPrefs.GetInt("Direction_Crossroad_" + crossroadIndex, -1); // مقدار پیش‌فرض -1 یعنی هنوز جهت انتخاب نشده
    }

    public float LoadDirectionTime(int crossroadIndex)
    {
        return PlayerPrefs.GetFloat("DirectionTime_Crossroad_" + crossroadIndex, 0f); // مقدار پیش‌فرض 0 یعنی هنوز زمان ثبت نشده
    }

    // متد برای پاک کردن داده‌های یک چهارراه خاص
    public void ClearCrossroadData(int crossroadIndex)
    {
        PlayerPrefs.DeleteKey("Direction_Crossroad_" + crossroadIndex);
        PlayerPrefs.DeleteKey("DirectionTime_Crossroad_" + crossroadIndex);
        PlayerPrefs.Save();
    }
}
