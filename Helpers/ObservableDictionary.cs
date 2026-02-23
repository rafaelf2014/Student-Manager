using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLPDS.Helpers {
    public class ObservableDictionary<TKey, TValue> :
        IDictionary<TKey, TValue>,
        ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable,
        INotifyCollectionChanged,
        INotifyPropertyChanged
        where TKey : notnull { 

        private readonly Dictionary<TKey, TValue> _dictionary = new();

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        // Parameterless constructor  
        public ObservableDictionary() {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        // Constructor with existing dictionary  
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary) {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        // OR: Constructor with IEnumerable (alternative if more flexibility is needed)  
        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> items) {
            _dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in items) {
                _dictionary.Add(item.Key, item.Value);
            }

        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args) =>
            CollectionChanged?.Invoke(this, args);

        public void Add(TKey key, TValue value) {
            _dictionary.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                new KeyValuePair<TKey, TValue>(key, value)
            ));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged("Item[]");
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        public bool Remove(TKey key) {
            if (_dictionary.TryGetValue(key, out TValue value) && _dictionary.Remove(key)) {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new KeyValuePair<TKey, TValue>(key, value)
                ));
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged("Item[]");
                OnPropertyChanged(nameof(Keys));
                OnPropertyChanged(nameof(Values));
                return true;
            }
            return false;
        }

        public void Clear() {
            _dictionary.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            OnPropertyChanged(nameof(Count));
            OnPropertyChanged("Item[]");
            OnPropertyChanged(nameof(Keys));
            OnPropertyChanged(nameof(Values));
        }

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        public ICollection<TKey> Keys => _dictionary.Keys;
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public TValue this[TKey key] {
            get => _dictionary[key];
            set {
                if (_dictionary.ContainsKey(key)) {
                    var oldItem = new KeyValuePair<TKey, TValue>(key, _dictionary[key]);
                    _dictionary[key] = value;
                    var newItem = new KeyValuePair<TKey, TValue>(key, value);
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace,
                        newItem,
                        oldItem
                    ));
                }
                else {
                    Add(key, value);
                }
                OnPropertyChanged("Item[]");
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);
        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.ContainsKey(item.Key) && EqualityComparer<TValue>.Default.Equals(_dictionary[item.Key], item.Value);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
    }
}
