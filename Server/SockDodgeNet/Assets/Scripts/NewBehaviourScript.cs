using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    class Animal
    {
        public string name;
        public int weight;

        public Animal(string name, int weight)
        {
            this.name = name;
            this.weight = weight;
        }
    }

    Animal lion = new Animal("사자", 100);
    Animal cat = new Animal("고양이", 10);

    List<Animal> animals = new List<Animal>();


    void Start()
    {
        animals.Add(lion);
        animals.Add(cat);

        for (int i = 0; i < animals.Count; i++)
        {
            Debug.Log($"{animals[i].name}");
        }

        animals.Remove(lion);
        animals.RemoveAt(0);

        Debug.Log(animals.Count);
    }
}
