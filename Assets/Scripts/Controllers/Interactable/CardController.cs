﻿using UnityEngine;

public class CardController : BaseController
{
    public BaseCard Card;
    
    public Vector3 TargetPosition = Vector3.zero;
    public Vector3 TargetRotation = Vector3.zero;

    private SpriteRenderer CardRenderer;
    private SpriteRenderer ComboGlowRenderer;

    private NumberController CostController;
    private NumberController AttackController;
    private NumberController AttributeController;

    private BoxCollider CardCollider;

    private string GlowType;

    public static CardController Create(BaseCard card)
    {
        GameObject cardObject = new GameObject(card.Name);
        
        BoxCollider cardCollider = cardObject.AddComponent<BoxCollider>();
        cardCollider.size = new Vector3(3.5f, 5.5f, 0f);

        CardController controller = cardObject.AddComponent<CardController>();
        controller.Card = card;
        controller.CardCollider = cardCollider;
        controller.GlowType = controller.GetGlowType();

        controller.Initialize();

        return controller;
    }
    
    public override void Initialize()
    {
        CostController = NumberController.Create("CostController", this.gameObject, new Vector3(-1.375f, 2.15f, 0f), 43);
        AttackController = NumberController.Create("AttackController", this.gameObject, new Vector3(-1.4f, -0.85f, 0f), 43);
        AttributeController = NumberController.Create("AttributeController", this.gameObject, new Vector3(1.5f, 0f, 0f), 43);

        CardRenderer = CreateRenderer("Card", Vector3.one, Vector3.zero, 42);
        
        ComboGlowRenderer = CreateRenderer("ComboGlow", Vector3.one * 3f, new Vector3(0.065f, -0.05f, 0f), 41);
        GreenGlowRenderer = CreateRenderer("GreenGlow", Vector3.one * 3f, new Vector3(0.065f, -0.05f, 0f), 40);

        CardRenderer.enabled = true;

        UpdateSprites();
        UpdateNumbers();

        CostController.SetEnabled(true);
    }

    public override void Remove()
    {
        CardRenderer.DisposeSprite();
        Destroy(CardRenderer);

        Destroy(GreenGlowRenderer);
        Destroy(ComboGlowRenderer);
    }

    public override void UpdateSprites()
    {
        // Loading the sprites into the SpriteRenderers
        CardRenderer.sprite = Resources.Load<Sprite>("Sprites/" + Card.Class.Name() + "/Cards/" + Card.TypeName());
        GreenGlowRenderer.sprite = SpriteManager.Instance.Glows["Card_" + GlowType + "_GreenGlow"];
        //ComboGlowRenderer.sprite = SpriteManager.Instance.Glows["Card_" + GlowType + "_ComboGlow"];
    }

    public override void UpdateNumbers()
    {
        if (Card.CurrentCost < Card.BaseCost)
        {
            CostController.UpdateNumber(Card.CurrentCost, "Green");
        }
        else if (Card.CurrentCost == Card.BaseCost)
        {
            CostController.UpdateNumber(Card.CurrentCost, "White");
        }
        else
        {
            CostController.UpdateNumber(Card.CurrentCost, "Red");
        }
    }

    public void SetRenderingOrder(int order)
    {
        CostController.SetRenderingOrder(order + 3);
        AttackController.SetRenderingOrder(order + 3);
        AttributeController.SetRenderingOrder(order + 3);

        CardRenderer.sortingOrder = order + 2;

        ComboGlowRenderer.sortingOrder = order + 1;
        GreenGlowRenderer.sortingOrder = order;
    }

    private string GetGlowType()
    {
        switch (Card.TypeName())
        {
            case "MinionCard":
                if (Card.As<MinionCard>().Rarity == CardRarity.Legendary)
                {
                    return "LegendaryMinion";
                }
                return "Minion";

            case "SpellCard":
                return "Spell";

            case "WeaponCard":
                return "Weapon";

            default:
                return "Normal";
        }
    }

    #region Unity Messages

    private ControllerStatus Status = ControllerStatus.Inactive;
    private bool IsHovering = false;

    private void Update()
    {
        float moveSpeed = 100f * Time.deltaTime;

        Vector3 targetZoomPosition = new Vector3(TargetPosition.x, 13f, 0f);

        switch (Status)
        {
            case ControllerStatus.Inactive:
                if (IsHovering && transform.localPosition.x == TargetPosition.x)
                {
                    transform.localScale = Vector3.one * 2f;
                    transform.localEulerAngles = Vector3.zero;
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetZoomPosition, moveSpeed);
                }
                else
                {
                    transform.localScale = Vector3.one;
                    transform.localEulerAngles = TargetRotation;
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPosition, moveSpeed);
                }
                break;

            case ControllerStatus.Dragging:
                transform.localScale = Vector3.one;
                transform.localEulerAngles = Vector3.zero;
                transform.position = Util.GetWorldMousePosition();
                break;

            case ControllerStatus.Targeting:    
                transform.localScale = Vector3.one;
                transform.localEulerAngles = Vector3.zero;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetZoomPosition, moveSpeed);
                break;
        }
    }

    private void OnMouseEnter()
    {
        IsHovering = true;
    }

    private void OnMouseExit()
    {
        IsHovering = false;
    }

    private void OnMouseDown()
    {
        switch (Card.GetCardType())
        {
            case CardType.Minion:
            case CardType.Weapon:
                Status = ControllerStatus.Dragging;
                break;

            case CardType.Spell:
                Status = ControllerStatus.Targeting;
                InterfaceManager.Instance.EnableArrow(this);
                break;
        }
    }

    private void OnMouseUp()
    {
        Status = ControllerStatus.Inactive;

        switch (Card.GetCardType())
        {
            case CardType.Minion:
            case CardType.Weapon:
                // TODO : Check position and play or not
                break;

            case CardType.Spell:
                InterfaceManager.Instance.DisableArrow();
                break;
        }
    }

    #endregion
}