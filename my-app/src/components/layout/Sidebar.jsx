import { NavLink } from "react-router-dom";

const links = [
    { to: "/dashboard", label: "Dashboard" },
    { to: "/inventory", label: "Inventory" },
    { to: "/settings", label: "Settings" },
];

export default function Sidebar({ open, onClose }) {
    return (
      <aside
        className={`fixed md:static inset-y-0 left-0 w-56 bg-slate-900 text-slate-100 p-4
          transform transition-transform md:translate-x-0
          ${open ? 'translate-x-0' : '-translate-x-full'}`}
      >
        <div className="text-xl font-semibold mb-6">TAMS</div>
        <nav className="space-y-1">
          {links.map((l) => (
            <NavLink
              key={l.to}
              to={l.to}
              onClick={onClose}
              className={({ isActive }) =>
                `block px-3 py-2 rounded ${isActive ? 'bg-slate-700' : 'hover:bg-slate-800'}`
              }
            >
              {l.label}
            </NavLink>
          ))}
        </nav>
      </aside>
    );
  }