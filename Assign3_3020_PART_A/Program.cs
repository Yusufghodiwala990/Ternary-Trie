/*----------------------------------------------------------------------*/

/* This console application implements a Ternary Trie. The code for most methods was taken from Dr Patrick's implementation.
 *  I added the Remove() method to delete a key in a ternary trie and adjust the structure appropriately.

/*----------------------------------------------------------------------*/

//Assignment 3 : PART 1, Ternary Trie by Yusuf Ghodiwala (0683640)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





// PART 1


// Using TernaryTrie implementation shown in class
namespace TrieTernaryTree
{
    public interface IContainer<T>
    {
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    //-------------------------------------------------------------------------

    public interface ITrie<T> : IContainer<T>
    {
        bool Insert(string key, T value);
        T Value(string key);
    }

    //-------------------------------------------------------------------------

    class Trie<T> : ITrie<T>
    {
        private Node root;                 // Root node of the Trie
        private int size;                  // Number of values in the Trie

        class Node
        {
            public char ch;                // Character of the key
            public T value;                // Value at Node; otherwise default
            public Node low, middle, high; // Left, middle, and right subtrees

            // Node
            // Creates an empty Node
            // All children are set to null
            // Time complexity:  O(1)

            public Node(char ch)
            {
                this.ch = ch;
                value = default(T);
                low = middle = high = null;
            }
        }

        // Trie
        // Creates an empty Trie
        // Time complexity:  O(1)

        public Trie()
        {
            MakeEmpty();
            size = 0;
        }

        // Public Insert
        // Calls the private Insert which carries out the actual insertion
        // Returns true if successful; false otherwise

        public bool Insert(string key, T value)
        {
            return Insert(ref root, key, 0, value);
        }

        // Private Insert
        // Inserts the key/value pair into the Trie
        // Returns true if the insertion was successful; false otherwise
        // Note: Duplicate keys are ignored
        // Time complexity:  O(n+L) where n is the number of nodes and 
        //                                L is the length of the given key

        private bool Insert(ref Node p, string key, int i, T value)
        {
            if (p == null)
                p = new Node(key[i]);

            // Current character of key inserted in left subtree
            if (key[i] < p.ch)
                return Insert(ref p.low, key, i, value);

            // Current character of key inserted in right subtree
            else if (key[i] > p.ch)
                return Insert(ref p.high, key, i, value);

            else if (i + 1 == key.Length)
            // Key found
            {
                // But key/value pair already exists
                if (!p.value.Equals(default(T)))
                    return false;
                else
                {
                    // Place value in node
                    p.value = value;
                    size++;
                    return true;
                }
            }

            else
                // Next character of key inserted in middle subtree
                return Insert(ref p.middle, key, i + 1, value);
        }

        //Method : Public Remove
        //Parameters: string key to be deleted from the Ternary trie
        //Return Type : Bool, true if deleted successfully
        //Description: This public Remove first checks if such a key exists in the Trie, then calls the private Remove() method
        //               to actually delete
        public bool Remove(string key)
        {
            if (!Contains(key))              // checking if the key exists
            {
                Console.WriteLine();
                Console.WriteLine("The key '{0}' does not exist in the trie", key);
                return false;
            }
            else                                   // if it exists, call the private remove method
                return remove(ref root, key, 0);

        }


        //Method : Private Remove
        //Parameters: Root Node(to start the traversal),string key and int i(to keep track of the current char in key)
        //Return Type : Bool, true if deleted successfully
        //Description: This method recursively traverses the ternary trie and deletes the key. If needed, it alters the structure of
        //              of the tree as well.
        private bool remove(ref Node p, string key, int i)
        {
            if (p == null)
                return false;  // empty tree


            if (key[i] < p.ch) // if char is less than current char
            {
                remove(ref p.low, key, i);   // recurse left(low) while keeping the i the same


            }

            else if (key[i] > p.ch)         // if char is greater than current char
            {
                remove(ref p.high, key, i);  // recurse right(high) while keeping the i the same



            }

            else if (i + 1 == key.Length)     // if we reach the end of the string key
            {
                if (!p.value.Equals(default(T)))      // if it's not a default value
                {

                    p.value = default(T);  // for a key that lies on the spine


                    if (p.middle != null) // case 2, p.middle is not empty
                    { }              // no need to adjust because it may have it's own children and we need to preserve the stem/spine

                    else if (p.low != null || p.high != null) // if we have 1 or 2 children
                    {
                        if (p.low != null) // when low exists
                        {
                            if (p.high != null) // when high exists as well
                            {
                                // We now look for the Max value we can find in the low subtree
                                Node curr = p;
                                Node prev = p;            // prev will stay one behind the max
                                Node max = p.low;        // max will move down to p.low


                                // traverse until there is no high
                                while (max.high != null)
                                {
                                    prev = max;              // will stay one behind
                                    max = max.high;

                                }
                                // replace the node with the max of the low subtree
                                p.ch = max.ch;
                                p.value = max.value;
                                p.middle = max.middle;       // pulling the middle along with it


                                if (prev.high == max)
                                    prev.high = max.low;      // if max was to the right of a node

                                    // EXAMPLE

                                    //           P
                                //          / \
                                //         B   Q        
                                //          \
                                //           A                
                                // max is 'A'. So our prev is B. Since A is directly to the right(high)
                                // of B, we just adjust the references of B.high.





                                else
                                    prev.low = max.low;      // if max was found in the very first node in the leftsubtree
                                //   meaning we could not traverse to the high subtree because the
                                //   the first node of leftsubtree was max.

                                // EXAMPLE for this case

                                //             G
                                //            / \
                                //           D   K       we want to delete G. We move to the left subtree and D is the Max value,
                                // because that's the furthest you can go(no D.high), D will get pulled up
                                //                         D.low in this example will become null, if D.low was not null then G.low
                                //                          would have referenced around D to make sure D.low children were not cut
                                //                            off
                                //                         

                            }
                            else
                                p = p.low;      // when p.high does not exist, only bring the low subtree up
                        }
                        else
                            p = p.high;         // when low subtree is null, and high subtree is not null, bring the p.high tree up



                    }
                    else            // when there are no children whatsoever
                    {
                        p = null; // cut off the node

                    }
                    size--;     // decrease the size
                }
                else
                    return false;     // there was a default value instead of a key value in the base node

            }

            else
            {
                remove(ref p.middle, key, i + 1);      // recurse in the middle subtree, and changing i to look at the next char

                // These operations will be performed while backing up from the recursive calls in the spine/stem

                if (p.middle == null && (p.high == null && p.low == null)) // case 1, no children.
                {
                    p = null;

                }

                else if (p.middle != null) // case 2, p.middle is not empty
                { }              // no need to adjust because it may have it's own children and we need to preserve the stem/spine

                else if (p.low != null || p.high != null) // if we have 1 or 2 children
                {
                    if (p.low != null) // when low exists
                    {
                        if (p.high != null) // when high exists as well
                        {
                            Node curr = p;
                            Node prev = p;
                            Node max = p.low;

                            while (max.high != null)
                            {
                                prev = max;              // will stay one behind
                                max = max.high;

                            }
                            p.ch = max.ch;
                            p.value = max.value;
                            p.middle = max.middle;              // replace the node with the max  of the low subtree
                            //        curr.ch = max.ch;

                            if (prev.high == max)
                                prev.high = max.low;      // if max had it's own children, we alter the prev's references around

                            else
                                prev.low = max.low;      // if max was found in the very first node in the leftsubtree

                        }
                        else
                            p = p.low;      // when p.high does not exist, only bring the low subtree up
                    }
                    else
                        p = p.high;         // when low subtree is null, and high subtree is not null, bring the p.high tree up



                }





            }
            return true;
        }



        public void printTree()
        {
            preTrav(root, 0);


        }


        private void preTrav(Node p, int indent)
        {


            if (p != null)
            {

                if (p.high != null)
                    preTrav(p.high, indent + 2);   //moving to the high of current node

                Console.WriteLine(new String(' ', indent) + p.ch + " " + p.value); // print the current nodes val

                if (p.middle != null)
                    preTrav(p.middle, indent + 6);  // move to the spine of the current node

                if (p.low != null)
                    preTrav(p.low, indent + 2);  //moving to the low of current node
            }




        }


        // Value
        // Returns the value associated with a key; otherwise default
        // Time complexity:  O(d) where d is the depth of the trie

        public T Value(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return the value if all characters of the key have been visited 
                    if (++i == key.Length)
                        return p.value;

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return default(T);   // Key too long
        }

        // Contains
        // Returns true if the given key is found in the Trie; false otherwise
        // Time complexity:  O(d) where d is the depth of the trie

        public bool Contains(string key)
        {
            int i = 0;
            Node p = root;

            while (p != null)
            {
                // Search for current character of the key in left subtree
                if (key[i] < p.ch)
                    p = p.low;

                // Search for current character of the key in right subtree           
                else if (key[i] > p.ch)
                    p = p.high;

                else // if (p.ch == key[i])
                {
                    // Return true if the key is associated with a non-default value; false otherwise 
                    if (++i == key.Length)
                        return !p.value.Equals(default(T));

                    // Move to next character of the key in the middle subtree   
                    p = p.middle;
                }
            }
            return false;        // Key too long
        }




        // MakeEmpty
        // Creates an empty Trie
        // Time complexity:  O(1)

        public void MakeEmpty()
        {
            root = null;
        }

        // Empty
        // Returns true if the Trie is empty; false otherwise
        // Time complexity:  O(1)

        public bool Empty()
        {
            return root == null;
        }

        // Size
        // Returns the number of Trie values
        // Time complexity:  O(1)

        public int Size()
        {
            return size;
        }

        // Public Print
        // Calls private Print to carry out the actual printing

        public void Print()
        {
            Print(root, "");
        }

        // Private Print
        // Outputs the key/value pairs ordered by keys
        // Time complexity:  O(n) where n is the number of nodes

        private void Print(Node p, string key)
        {
            if (p != null)
            {
                Print(p.low, key);
                if (!p.value.Equals(default(T)))
                    Console.WriteLine(key + p.ch + " " + p.value);
                Print(p.middle, key + p.ch);
                Print(p.high, key);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Trie<int> T;
            T = new Trie<int>();

            T.Insert("cow", 50);
            T.Insert("cop", 10);
            T.Insert("coward", 99);
            T.Insert("cower", 10);
            T.Insert("did", 22);
            T.Insert("done", 45);
            T.Insert("dare", 49);
            T.Insert("dba", 88);
            T.Insert("bee", 1);
            T.Insert("abc", 56);
            T.Insert("abb", 10);
            T.Insert("add", 13);


            Console.WriteLine("After all the insertions, the keys/values are:");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());






            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();




            // Testing out Remove()

            Console.WriteLine("Now doing a few removes");
            Console.WriteLine();


            Console.WriteLine("First Remove is 'did' because a case 3 will occur where high and low both exist in that subtree");

            // there will be 2 subtrees, high and low, when deleting 'did'. Max value of low subtree will be chosen to replace
            //  the deleted key.
            T.Remove("did");



            Console.WriteLine("After the removal of 'did', the keys/values are:");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());




            // case when low subtree get's pulled up to where they key being removed was.
            T.Remove("abc");
            Console.WriteLine("After the removal of 'abc', the keys/values are:");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());





            // case when removing a key from the stem, but there are other keys down the stem
            T.Remove("cow");
            Console.WriteLine("After the removal of 'cow', the keys/values are:");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());





            // case when high subtree gets pulled up to where the the key being removed was
            T.Remove("abb");
            Console.WriteLine("After the removal of 'abb', the keys/values are:");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());



            // another case of high or low subtree being the key replacements of the key removed.
            T.Remove("bee");
            Console.WriteLine("After the removal of 'bee', the keys/values are:");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());



            // trying to remove a key which does not exist.
            T.Remove("pop");
            Console.WriteLine();

            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());



            // removing another key in the stem. if there are other branching low or high subtree from down the spine, it
            //  will replace the key being removed.
            T.Remove("coward");
            Console.WriteLine("After the removal of 'coward' which lies in the stem/spine, the keys/values are:");
            Console.WriteLine();


            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());




            // Testing  Value (although we didn't have to implement any other methods of trie)


            Console.WriteLine("Now testing, Value() to find the value of the key 'dba'");
            Console.WriteLine("The value is {0}", T.Value("dba"));




            Console.WriteLine("Now testing, Value() to find the value of the key 'lol'");
            Console.WriteLine("The value is {0}", T.Value("lol"));





            // removing another key from the stem
            T.Remove("cower");
            Console.WriteLine("After the removal of 'cower' which lies in the stem/spine, the keys/values are:");
            Console.WriteLine();


            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());




            // this is the last iteration of the stem keys removal, a wholly new key will take it's place( max value of low subtree)
            T.Remove("cop");
            Console.WriteLine("After the removal of 'cop' which lies in the stem/spine, the keys/values are:");
            Console.WriteLine();


            T.Print();
            Console.WriteLine();
            Console.WriteLine("The tree structure: (Note, it's at a 90 deg, the middle trees are slanted");
            Console.WriteLine("but a slanted straight line can be drawn to see the middle children");
            Console.WriteLine();
            T.printTree();

            Console.WriteLine("The size is : {0}", T.Size());


        }
    }
}
