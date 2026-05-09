import { NavLink } from "react-router-dom";

const links = [
  { to: "/dashboard", label: "Dashboard" },
  { to: "/inventory", label: "Inventory" },
  { to: "/settings", label: "Settings" },
];

export default function Sidebar({ open, onClose }) {
  return (
    <>
      {open && (
        <div
          onClick={onClose}
          className="fixed inset-0 bg-black/40 z-30 md:hidden"
          aria-hidden="true"
        />
      )}
      <aside
        className={`fixed md:static inset-y-0 left-0 w-56 bg-slate-900 text-slate-100 p-4 z-40
              transform transition-transform md:translate-x-0
              ${open ? 'translate-x-0' : '-translate-x-full'}`}
      >
        <div className="flex items-center justify-between mb-6">
          <div className="text-xl font-semibold">TAMS</div>
          <button
            onClick={onClose}
            className="md:hidden p-1 rounded hover:bg-slate-800"
            aria-label="Close menu"
          >
            ✕
          </button>
        </div>
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
    </>
  );
}