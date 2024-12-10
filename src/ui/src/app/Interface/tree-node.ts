export interface TreeNode {
    label: string; // Display name of the node
    value: any;    // The unique identifier for the node (often an ID)
    children?: TreeNode[]; // Optional: Children nodes (subordinates)
    expanded?: boolean; // Optional: Whether the node is expanded in the UI
    selected?: boolean; // Optional: Whether the node is selected
    disabled?: boolean; // Optional: Whether the node is disabled
    style?: string; // Optional: Custom styling
  }
  