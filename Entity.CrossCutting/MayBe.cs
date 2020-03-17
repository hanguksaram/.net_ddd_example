namespace Entity.CrossCutting
{
    //todo move to Core project
    public class MayBe<T> where T:class
    {
        public static MayBe<T> Empty => new MayBe<T>();

        public MayBe(T value)
        {
            IsEmpty = value == default(T);
            Value = value;
        }
        
        private MayBe()
        {
            IsEmpty = true;
        }

        public T Value { get; } = default(T);
        public bool IsEmpty { get; }

    }
}