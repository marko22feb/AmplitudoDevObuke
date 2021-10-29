using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InteractableObject objectToInteractWith;
    public GameObject InteractCanvasPrefab;
    private GameObject spawnedCanvas;
    private Animator InventoryAC;
    public Animator WeaponSlotAC;
    private bool InventoryAnimation = false;
    private StatComponent statComp;
    public bool TestingAndroid = false;

    private AbilityComponent ability;
    private InputActions keybindings;
    public Abilities equipedAbility = Abilities.NULL;
    public float abilityRateOfFire = 0.2f;
    private bool isUsingAbility = false;

    public void ActivateInteract(InteractableObject ob)
    {
        if (objectToInteractWith == ob) return;
        objectToInteractWith = ob;
        if (ob != null)
            spawnedCanvas = Instantiate(InteractCanvasPrefab, objectToInteractWith.transform);
        else Destroy(spawnedCanvas);
    }

    private void Awake()
    {
        InventoryAC = GameObject.Find("InventoryPanel").GetComponent<Animator>();
        statComp = GetComponent<StatComponent>();
        TestingAndroid = GetComponent<MovementScript>().TestingAndroid;
    }

    private void Start()
    {
        ability = GetComponent<AbilityComponent>();
        keybindings = InputController.inputs.keybindings;
        keybindings.Actions.Debug.started += OnDebug;
        keybindings.Actions.Shoot.started += Shoot;

    }

    private void OnDebug(InputAction.CallbackContext obj)
    {
        Debug.Log("Pressed Keybind");
    }

    private void Update()
    {
        if (!GameController.control.IsInputEnabled) return;

        if (Application.platform != RuntimePlatform.Android && !TestingAndroid)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Interact();
            }

            if (Input.GetButtonDown("Inventory"))
            {
                ClickedInventory();
            }

            if (Input.GetButtonDown("Consumable Left"))
            {
                UseConsumable(0);
            }

            if (Input.GetButtonDown("Consumable Right"))
            {
                UseConsumable(1);
            }

            if (Input.GetButtonDown("Melee"))
            {
                Melee();
            }

            if (Input.GetButtonDown("Shoot"))
            {

            }

            if (Input.GetButtonDown("Settings"))
            {
               Time.timeScale = 0f;
               GameController.control.IsInputEnabled = false;
               GameController.control.settingsCanvas.GetComponent<Canvas>().enabled = true;
            }

        }
    }

    public void UseConsumable(int slot)
    {
        if (GameController.control.equipData[slot].Amount > 0)
        {
            ItemData itemData = Inventory.inv.GetItem(GameController.control.equipData[slot].ItemID);
            if (statComp.CurrentHealth == statComp.MaximumHealth) return;
            statComp.ModifyBy(itemData.consumeData.stat, itemData.consumeData.changeBy);
            InventoryData slotData = GameController.control.equipData[slot];
            slotData.Amount -= 1;

            if (slotData.Amount < 1) slotData = new InventoryData(-1);
            GameController.control.equipData[slot] = slotData;
            Inventory.inv.RefreshEquipableSlots();
        }
    }

    public void Interact()
    {
        if (objectToInteractWith != null)
        {
            objectToInteractWith.OnInteract();
        }
    }

    public void ClickedInventory()
    {
        if (!InventoryAnimation)
        {
            InventoryAC.SetTrigger("ClickedInventory");
            InventoryAnimation = true;
        }
    }

    public void Melee()
    {
        if (WeaponSlotAC.transform.childCount > 0)
        {
            WeaponSlotAC.SetTrigger("Attack");
            int random = Random.Range(0, 2);
            AudioClip swoosh;
            if (random == 0) swoosh = Resources.Load("Audio_SFX/DM-CGS-46") as AudioClip; else swoosh = Resources.Load("Audio_SFX/DM-CGS-47") as AudioClip;
            AudioSource.PlayClipAtPoint(swoosh, new Vector3(0, 0, 0));
        }
    }

    public void Shoot(InputAction.CallbackContext obj)
    {
        if (equipedAbility != Abilities.NULL && !isUsingAbility)
        {
            ability.OnAbilityUse(equipedAbility);
            StartCoroutine(rateOfFire(abilityRateOfFire));
        }
    }

    public void CanReEnterInventory ()
    {
        InventoryAnimation = false;
    }

    IEnumerator rateOfFire(float delay)
    {
        isUsingAbility = true;
        yield return new WaitForSeconds(delay);
        isUsingAbility = false;
    }
}
