export default function PriceLookupButton({ onLookup, loading, disabled }) {
  return (
    <button
      type="button"
      onClick={onLookup}
      disabled={loading || disabled}
      className="bg-slate-900 text-white px-3 py-1.5 rounded hover:bg-slate-800 text-sm disabled:opacity-50 inline-flex items-center gap-2"
    >
      {loading && (
        <span
          aria-hidden="true"
          className="inline-block w-3.5 h-3.5 border-2 border-white border-t-transparent rounded-full animate-spin"
        />
      )}
      {loading ? 'Looking up…' : 'Run price lookup'}
    </button>
  );
}
