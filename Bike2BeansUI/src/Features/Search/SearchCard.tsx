type Props = {
    text: any
}


export function SearchCard({ text }: Props) {
    return (
        <div className="searchbar-result">
            {text}
        </div>
    )
}
