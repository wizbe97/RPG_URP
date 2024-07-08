using UnityEngine;

public class Consumable : MonoBehaviour
{
   public Item item;

   //WILL AUTOMATICALLY ASSIGN A UNIQUE ID FOR EACH ITEM BASED ON ITEM NAME AND IT'S POSITION SO IT IS ALWAYS A UNIQUE ID
   //SO WE CAN KEEP TRACK AND DISABLE THE ONES THAT HAVE BEEN COLLECTED ALREADY
   public string ID;

   private void Awake()
   {
      if (string.IsNullOrEmpty(ID))
      {
         ID = gameObject.name + transform.position;
      }
   }
}
