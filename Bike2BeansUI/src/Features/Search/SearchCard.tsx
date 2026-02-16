type Props = {
    text: any
}


export function SearchCard({ text }: Props) {
    return (
        <div className="searchbox-result">
            {text}
        </div>
    )
}
