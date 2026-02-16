type Props = {
    text: any
}


export function SearchResultCard({ text }: Props) {
    return (
        <div className="searchbox-result">
            {text}
        </div>
    )
}
