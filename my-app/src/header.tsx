import './header.css';

function Header() {
    return (
        <header className="header">
            <h1 className="header-title">TAMS</h1>
            <div className="header-nav">
                <a href="#">Inventory</a>
                <a href="#">About</a>
            </div>
        </header>
    );
}

export default Header;