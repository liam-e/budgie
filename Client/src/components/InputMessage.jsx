const InputMessage = ({ children }) => {
  return (
    <>
      {children ? (
        <p className="visible text-sm line-clamp-2 w-60">{children}</p>
      ) : (
        <p className="invisible text-sm line-clamp-2 w-60">[Placeholder]</p>
      )}
    </>
  );
};

export default InputMessage;
