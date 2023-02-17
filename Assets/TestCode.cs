using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public List<TestEntity> list;
    public int sum;
    public void Awake()
    {
        TestEntity x1 = new TestEntity("���1", 1);
        TestEntity x2 = new TestEntity("���2", 2);
        TestEntity x3 = new TestEntity("���3", 0);
        TestEntity x4 = new TestEntity("���4", 4);
        TestEntity x5 = new TestEntity("���5", 5);
        list = new List<TestEntity>();
        list.Add(x1);
        list.Add(x2);
        list.Add(x3);
        list.Add(x4);
        list.Add(x5);
        sum = 0;
        sum = list.Where(x => !x.CheckZeroLevel()).Sum(x => x.level);

        // ���������� 12 ���
        Debug.Log(sum);
    }



}

public class TestEntity
{
    public string name;
    public int level;

    public TestEntity(string name, int level) 
    { 
        this.name = name;
        this.level = level; 
    }
    public bool CheckZeroLevel()
    {
        return (this.level == 0);
    }

}

