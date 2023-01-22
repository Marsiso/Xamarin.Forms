using SQLite;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Discovery.Models;

[Table("Photos")]
public sealed class PhotoEntity : INotifyPropertyChanged, ICloneable
{
    private bool isBlackListed;
    private bool isFavourite;
    private bool isVisible = true;
    private string photographer = default!;
    private string url = default!;
    private string alt = default!;
    private string category = default!;
    private int id;

    public event PropertyChangedEventHandler? PropertyChanged;

    [Column("pk_photo")]
    [PrimaryKey]
    public int Id
    {
        get => id;
        set
        {
            if (id != value)
            {
                id = value;

                OnPropertyChanged(nameof(Id));
            }
        }
    }



    [Column("photo_alt")]
    [NotNull]
    public string Alt
    {
        get => alt;
        set
        {
            if (alt != value)
            {
                alt = value;

                OnPropertyChanged(nameof(Alt));
            }
        }
    }

    [Column("photo_url")]
    [NotNull]
    public string Url
    {
        get => url;
        set
        {
            if (url != value)
            {
                url = value;

                OnPropertyChanged(nameof(Url));
            }
        }
    }

    [Column("photographer")]
    [NotNull]
    public string Photographer
    {
        get => photographer;
        set
        {
            if (photographer != value)
            {
                photographer = value;

                OnPropertyChanged(nameof(Photographer));
            }
        }
    }

    [Column("is_visible")]
    [NotNull]
    public bool IsVisible
    {
        get => isVisible;
        set
        {
            if (isVisible != value)
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
    }

    [Column("is_favourite")]
    [NotNull]
    public bool IsFavourite
    {
        get => isFavourite;
        set
        {
            if (isFavourite != value)
            {
                isFavourite = value;

                OnPropertyChanged(nameof(IsFavourite));
            }
        }
    }


    [Column("is_blackListed")]
    [NotNull]
    public bool IsBlackListed
    {
        get => isBlackListed;
        set
        {
            if (isBlackListed != value)
            {
                isBlackListed = value;

                OnPropertyChanged(nameof(IsBlackListed));
            }
        }
    }

    [Column("category")]
    [NotNull]
    public string Category
    {
        get => category;
        set
        {
            if (category != value)
            {
                category = value;

                OnPropertyChanged(nameof(Category));
            }
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
