namespace BoxRender
{
    class Rectangle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public int AttachedToSide { get; set; }
        public float HingeOffset { get; set; }
        public bool IsRoot { get; set; }

        public Rectangle()
        {
            IsRoot = false;
        }

    }
}
