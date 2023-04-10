using System.Collections.Generic;

namespace UiEMG.Response
{
    /// <summary>
    ///     ReceiveData Method를 통해 얻은 값들을 사용에 맞게 재배치하는 역할
    /// </summary>
    public class ArrayData
    {
        /// <summary>
        ///     IEnumerable: ReceiveData 메서드에 존재하는 값들(value)을 단순 반복할 수 있도록 지원하는 열거자를 노출
        /// </summary>
        public ArrayData(IEnumerable<ReceiveData> values)
        {
            foreach (var value in values)
            {
                EMG1.Add(value.EMG1);
                EMG2.Add(value.EMG2);
            }
        }

        public List<int> EMG1 { get; } = new List<int>();
        public List<int> EMG2 { get; } = new List<int>();

    }

}
