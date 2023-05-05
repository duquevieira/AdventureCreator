using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MeadowGames.UINodeConnect4.UICContextMenu
{
    public class DuplicateNodeButtonItem : ContextItem
    {
        Button _button;

        public void Duplicate()
        {
            for (int i = UICSystemManager.selectedElements.Count - 1; i >= 0; i--)
            {
                Node nodeToDuplicate = UICSystemManager.selectedElements[i] as Node;
                if (nodeToDuplicate)
                {
                    ContextMenu.GraphManager.InstantiateNode(nodeToDuplicate, nodeToDuplicate.transform.position + new Vector3(10, 10, 0));
                }
            }
            ContextMenu.UpdateContextMenu();
        }

        public override void OnChangeSelection()
        {
            int nodeCount = 0;
            for (int i = UICSystemManager.selectedElements.Count - 1; i >= 0; i--)
            {
                Node node = UICSystemManager.selectedElements[i] as Node;
                if (node)
                {
                    nodeCount++;
                }
            }
            gameObject.SetActive(nodeCount > 0);
        }

        protected override void Awake()
        {
            base.Awake();
            _button = GetComponent<Button>();
        }

        void OnEnable()
        {
            _button.onClick.AddListener(Duplicate);
        }

        void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}